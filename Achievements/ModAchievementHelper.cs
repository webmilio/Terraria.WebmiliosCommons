using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Achievements;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace WebmilioCommons.Achievements
{
    public class ModAchievementHelper
    {
        public const string
            ACHIEVEMENT_PREFIX = "Achievements.",
            ACHIEVEMENT_FRIENDLYNAME_KEY = ACHIEVEMENT_PREFIX + ".{0}_Name",
            ACHIEVEMENT_DESCRIPTION_KEY = ACHIEVEMENT_PREFIX + ".{0}_Description";

        public const BindingFlags PRIVATE_FIELD_BINDING_FLAGS = BindingFlags.NonPublic | BindingFlags.Instance;

        private static FieldInfo
            _achievements,
            _achievementIcon, _achievementIconBorders, _large,
            _locked, _iconFrame, _iconFrameLocked, _iconFrameUnlocked;

        private static MethodInfo _localizedTextSetValue;

        private static string _letThrough;

        private static Dictionary<Achievement, ModAchievement> _loadedAchievements;


        #region Loading/Unloading

        public static void PostSetupContent()
        {
            _loadedAchievements = new Dictionary<Achievement, ModAchievement>();

            Type
                achievementType = typeof(Achievement),
                uiAchievementEntryType = typeof(UIAchievementListItem);


            _achievements = typeof(AchievementManager).GetField(nameof(_achievements), PRIVATE_FIELD_BINDING_FLAGS);

            _localizedTextSetValue = typeof(LocalizedText).GetMethod("SetValue", PRIVATE_FIELD_BINDING_FLAGS);

            _achievementIcon = uiAchievementEntryType.GetField(nameof(_achievementIcon), PRIVATE_FIELD_BINDING_FLAGS);
            _achievementIconBorders = uiAchievementEntryType.GetField(nameof(_achievementIconBorders), PRIVATE_FIELD_BINDING_FLAGS);
            _large = uiAchievementEntryType.GetField(nameof(_large), PRIVATE_FIELD_BINDING_FLAGS);

            _locked = uiAchievementEntryType.GetField(nameof(_locked), PRIVATE_FIELD_BINDING_FLAGS);
            _iconFrame = uiAchievementEntryType.GetField(nameof(_iconFrame), PRIVATE_FIELD_BINDING_FLAGS);
            _iconFrameLocked = uiAchievementEntryType.GetField(nameof(_iconFrameLocked), PRIVATE_FIELD_BINDING_FLAGS);
            _iconFrameUnlocked = uiAchievementEntryType.GetField(nameof(_iconFrameUnlocked), PRIVATE_FIELD_BINDING_FLAGS);

            // Hooking
            On.Terraria.Achievements.AchievementManager.Register += AchievementManagerOnRegister;
            On.Terraria.Achievements.AchievementManager.RegisterIconIndex += AchievementManagerOnRegisterIconIndex;
            On.Terraria.Achievements.AchievementManager.GetIconIndex += AchievementManagerOnGetIconIndex;

            On.Terraria.GameContent.UI.Elements.UIAchievementListItem.ctor += UIAchievementListItemOnCtor;

            // Loading
            ModAchievementLoader.Instance.TryLoad(); // Redundant, since singletons always load on call.
        }

        public static void Unload()
        {
            if (_achievements == null)
            {
                WebmilioCommonsMod.Instance.Logger.Error("Achievements getter was found to be null; canceling unload.");
                return;
            }

            Dictionary<string, Achievement> vanillaAchievementsDictionary = (Dictionary<string, Achievement>)_achievements.GetValue(Main.Achievements);

            foreach (KeyValuePair<Achievement, ModAchievement> kvp in _loadedAchievements)
                vanillaAchievementsDictionary.Remove(kvp.Key.Name);

            On.Terraria.Achievements.AchievementManager.RegisterIconIndex -= AchievementManagerOnRegisterIconIndex;
            On.Terraria.Achievements.AchievementManager.GetIconIndex -= AchievementManagerOnGetIconIndex;

            On.Terraria.GameContent.UI.Elements.UIAchievementListItem.ctor -= UIAchievementListItemOnCtor;
        }

        #endregion


        /// <summary>
        /// If for some reason the ModAchievement Loader has not loaded the desired achievement, you can use this.
        /// Under no circumstances should you load an achievement that is already loaded.
        /// </summary>
        public static void RegisterAchievement(ModAchievement modAchievement, Type type)
        {
            Achievement achievement = new Achievement(type.FullName);

            _localizedTextSetValue.Invoke(achievement.FriendlyName, new object[] { modAchievement.Name });
            _localizedTextSetValue.Invoke(achievement.Description, new object[] { modAchievement.Description });

            achievement.SetCategory(modAchievement.Category);

            modAchievement.GameAchievement = achievement;
            modAchievement.SetDefaults();

            achievement.AddConditions(modAchievement.conditions.ToArray());

            _loadedAchievements.Add(achievement, modAchievement);

            Main.Achievements.Register(achievement);
        }


        public static ModAchievement GetModAchievement<T>()
        {
            foreach (KeyValuePair<Achievement, ModAchievement> kvp in _loadedAchievements)
                if (kvp.Key.Name.Equals(typeof(T).FullName, StringComparison.CurrentCultureIgnoreCase))
                    return kvp.Value;

            return null; 
        }

        public static ModAchievement GetModAchievement(string name)
        {
            foreach (KeyValuePair<Achievement, ModAchievement> kvp in _loadedAchievements)
                if (kvp.Value.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase))
                    return kvp.Value;

            return null;
        }


        #region Hooking

        private static void AchievementManagerOnRegister(On.Terraria.Achievements.AchievementManager.orig_Register orig, AchievementManager self, Achievement achievement)
        {
            orig(self, achievement);

            if (WebmilioCommonsMod.Instance.ClientConfiguration.ResetAchievements)
            {
                achievement.ClearProgress();
                achievement.ClearTracker();
            }
        }

        private static void UIAchievementListItemOnCtor(On.Terraria.GameContent.UI.Elements.UIAchievementListItem.orig_ctor orig, Terraria.GameContent.UI.Elements.UIAchievementListItem self, Achievement achievement, bool largeForOtherLanguages)
        {
            if (_letThrough != achievement.Name)
            {
                _letThrough = achievement.Name;
                orig(self, achievement, largeForOtherLanguages);
            }

            if (_loadedAchievements.ContainsKey(achievement))
            {
                Rectangle originalFrameLocked = (Rectangle)_iconFrameLocked.GetValue(self);

                _iconFrameLocked.SetValue(self, new Rectangle(originalFrameLocked.X + 2, originalFrameLocked.Y, originalFrameLocked.Width, originalFrameLocked.Height));

                GetAchievementIcon(self).Remove();

                UIImageFramed achievementIcon = new UIImageFramed(ModContent.GetTexture(_loadedAchievements[achievement].TexturePath), (Rectangle)_iconFrame.GetValue(self));

                bool large = (bool)_large.GetValue(self);

                float
                    xOffset = large ? 6 : 0,
                    yOffset = large ? 12 : 0;

                achievementIcon.Left.Set(xOffset, 0f);
                achievementIcon.Top.Set(yOffset, 0f);

                _achievementIcon.SetValue(self, achievementIcon);
                self.Append(achievementIcon);

                UIImage achievementIconBorders = GetAchievementIconBorders(self);
                achievementIconBorders.Remove();
                self.Append(achievementIconBorders);
            }
        }

        // Redundant checks to make sure Terraria doesn't do the big dumb.
        private static void AchievementManagerOnRegisterIconIndex(On.Terraria.Achievements.AchievementManager.orig_RegisterIconIndex orig, Terraria.Achievements.AchievementManager self, string achievementName, int iconIndex) =>
            orig(self, achievementName, achievementName.StartsWith(ACHIEVEMENT_PREFIX) ? 0 : iconIndex);

        private static int AchievementManagerOnGetIconIndex(On.Terraria.Achievements.AchievementManager.orig_GetIconIndex orig, Terraria.Achievements.AchievementManager self, string achievementName)
        {
            return achievementName.StartsWith(ACHIEVEMENT_PREFIX) ? 0 : orig(self, achievementName);
        }

        #endregion


        private static UIImageFramed GetAchievementIcon(UIAchievementListItem achievementUIItem) => _achievementIcon.GetValue(achievementUIItem) as UIImageFramed;

        private static UIImage GetAchievementIconBorders(UIAchievementListItem achievementUIItem) => _achievementIconBorders.GetValue(achievementUIItem) as UIImage;
    }
}
