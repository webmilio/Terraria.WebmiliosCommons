using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Terraria;
using Terraria.Achievements;
using Terraria.Social;
using Terraria.Utilities;

namespace WebmilioCommons.Achievements.Helper.Proxies
{
    public sealed class AchievementManagerProxy : IDisposable
    {
        private readonly FieldInfo _achievements, _cryptoKey, _savePath, _cloud, _serializerSettings;
        private object _ioLock;

        private Dictionary<string, StoredAchievement> _unfoundAchievements;

        public AchievementManagerProxy()
        {
            Type achievementsManagerType = Manager.GetType();

            _achievements = achievementsManagerType.GetField(nameof(_achievements), ModAchievementHelper.PRIVATE_FIELD_BINDING_FLAGS);
            _cryptoKey = achievementsManagerType.GetField(nameof(_cryptoKey), ModAchievementHelper.PRIVATE_FIELD_BINDING_FLAGS);

            _savePath = achievementsManagerType.GetField(nameof(_savePath), ModAchievementHelper.PRIVATE_FIELD_BINDING_FLAGS);
            _cloud = achievementsManagerType.GetField(nameof(_cloud), ModAchievementHelper.PRIVATE_FIELD_BINDING_FLAGS);

            _serializerSettings = achievementsManagerType.GetField(nameof(_serializerSettings), ModAchievementHelper.PRIVATE_FIELD_BINDING_FLAGS);

            On.Terraria.Achievements.AchievementManager.Register += AchievementManager_OnRegister;
            On.Terraria.Achievements.AchievementManager.RegisterIconIndex += AchievementManager_OnRegisterIconIndex;
            On.Terraria.Achievements.AchievementManager.GetIconIndex += AchievementManager_OnGetIconIndex;

            On.Terraria.Achievements.AchievementManager.Save += AchievementManager_OnSave;
            On.Terraria.Achievements.AchievementManager.Load += AchievementManager_OnLoad;

            _ioLock = new object();
        }


        private void AchievementManager_OnSave(On.Terraria.Achievements.AchievementManager.orig_Save orig, AchievementManager self)
        {
            orig(self);

            FakeSave();
        }

        private void AchievementManager_OnLoad(On.Terraria.Achievements.AchievementManager.orig_Load orig, AchievementManager self)
        {
            orig(self);

            // Clone
            VanillaAchievements = new Dictionary<string, Achievement>(Achievements);
        }


        public void FakeLoad()
        {
            lock (_ioLock)
            {
                _unfoundAchievements = new Dictionary<string, StoredAchievement>();

                if (!FileUtilities.Exists(ModdedSavePath, false))
                    return;

                byte[] buffer = FileUtilities.ReadAllBytes(ModdedSavePath, false);

                try
                {
                    using (MemoryStream memoryStream = new MemoryStream(buffer))
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, new RijndaelManaged().CreateDecryptor(CryptoKey, CryptoKey), CryptoStreamMode.Read))
                        {
                            using (BsonReader bsonReader = new BsonReader(cryptoStream))
                            {
                                JsonSerializer jsonSerializer = JsonSerializer.Create(SerializerSettings);
                                Dictionary<string, StoredAchievement> storedAchievements = jsonSerializer.Deserialize<Dictionary<string, StoredAchievement>>(bsonReader);

                                foreach (KeyValuePair<string, StoredAchievement> storedAchievement in storedAchievements)
                                    if (!Achievements.ContainsKey(storedAchievement.Key) || storedAchievement.Key.Contains("."))
                                        _unfoundAchievements.Add(storedAchievement.Key, storedAchievement.Value);
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    FileUtilities.Delete(SavePath, false);
                    return;
                }

                if (_unfoundAchievements == null)
                    return;

                foreach (KeyValuePair<string, StoredAchievement> achievement in _unfoundAchievements)
                    if (Achievements.ContainsKey(achievement.Key))
                        Achievements[achievement.Key].Load(achievement.Value.Conditions);
            }
        }

        public void FakeSave()
        {
            lock (_ioLock)
            {
                try
                {
                    if (!FileUtilities.Exists(ModdedSavePath, false))
                        File.Create(ModdedSavePath).Close();

                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, new RijndaelManaged().CreateEncryptor(CryptoKey, CryptoKey), CryptoStreamMode.Write))
                        {
                            using (BsonWriter bsonWriter = new BsonWriter(cryptoStream))
                            {
                                JsonSerializer jsonSerializer = JsonSerializer.Create(SerializerSettings);

                                jsonSerializer.Serialize(bsonWriter, Achievements);

                                if (_unfoundAchievements != null)
                                    foreach (KeyValuePair<string, StoredAchievement> storedAchievement in _unfoundAchievements)
                                        if (!Achievements.ContainsKey(storedAchievement.Key))
                                            jsonSerializer.Serialize(bsonWriter, storedAchievement.Value);

                                bsonWriter.Flush();
                                cryptoStream.FlushFinalBlock();
                                FileUtilities.WriteAllBytes(ModdedSavePath, memoryStream.ToArray(), false);
                            }
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
        }


        #region Hooks

        private static void AchievementManager_OnRegister(On.Terraria.Achievements.AchievementManager.orig_Register orig, AchievementManager self, Achievement achievement)
        {
            try
            {
                orig(self, achievement);

                if (WebmilioCommonsMod.Instance.ClientConfiguration.ResetAchievements)
                {
                    achievement.ClearProgress();
                    achievement.ClearTracker();
                }
            }
            catch
            {
                throw new Exception("An error occured while initializing achievements. This is sometimes due to updating Webmilio's Commons in certain scenarios. Restarting the game should fix the problem.");
            }
        }

        private static void AchievementManager_OnRegisterIconIndex(On.Terraria.Achievements.AchievementManager.orig_RegisterIconIndex orig, Terraria.Achievements.AchievementManager self, string achievementName, int iconIndex) =>
            orig(self, achievementName, achievementName.StartsWith(ModAchievementHelper.ACHIEVEMENT_PREFIX) ? 0 : iconIndex);

        private int AchievementManager_OnGetIconIndex(On.Terraria.Achievements.AchievementManager.orig_GetIconIndex orig, AchievementManager self, string achievementName) =>
            achievementName.StartsWith(ModAchievementHelper.ACHIEVEMENT_PREFIX) ? 0 : orig(self, achievementName);

        #endregion


        public void Dispose()
        {
            On.Terraria.Achievements.AchievementManager.Register -= AchievementManager_OnRegister;
            On.Terraria.Achievements.AchievementManager.RegisterIconIndex -= AchievementManager_OnRegisterIconIndex;
            On.Terraria.Achievements.AchievementManager.GetIconIndex -= AchievementManager_OnGetIconIndex;

            On.Terraria.Achievements.AchievementManager.Save -= AchievementManager_OnSave;
            On.Terraria.Achievements.AchievementManager.Load -= AchievementManager_OnLoad;
        }


        public AchievementManager Manager => Main.Achievements;

        public Dictionary<string, Achievement> Achievements => _achievements.GetValue(Manager) as Dictionary<string, Achievement>;
        internal Dictionary<string, Achievement> VanillaAchievements { get; private set; }

        public byte[] CryptoKey => _cryptoKey.GetValue(Manager) as byte[];

        public string SavePath => _savePath.GetValue(Manager) as string;
        public string ModdedSavePath => Main.SavePath + Path.DirectorySeparatorChar + "moddedachievements.dat";

        public bool Cloud => (bool)_cloud.GetValue(Manager);

        public JsonSerializerSettings SerializerSettings => _serializerSettings.GetValue(Manager) as JsonSerializerSettings;
    }
}