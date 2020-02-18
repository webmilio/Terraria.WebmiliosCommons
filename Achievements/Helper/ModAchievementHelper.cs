using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Achievements;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.ModLoader;
using WebmilioCommons.Achievements.Helper.Proxies;

namespace WebmilioCommons.Achievements.Helper
{
    // TODO Change to use normal achievement file (append unloaded achievements at the end)
    internal class ModAchievementHelper
    {
        public const string
            ACHIEVEMENT_PREFIX = "Achievements.",
            ACHIEVEMENT_FRIENDLYNAME_KEY = ACHIEVEMENT_PREFIX + ".{0}_Name",
            ACHIEVEMENT_DESCRIPTION_KEY = ACHIEVEMENT_PREFIX + ".{0}_Description";

        public const BindingFlags PRIVATE_FIELD_BINDING_FLAGS = BindingFlags.NonPublic | BindingFlags.Instance;

        private static FieldInfo
            _achievementIcon, _achievementIconBorders, _large,
            _locked, _iconFrame, _iconFrameLocked, _iconFrameUnlocked;

        private static MethodInfo _localizedTextSetValue;

        private static string _letThrough;

        private static Dictionary<Achievement, ModAchievement> _loadedAchievements;


        #region Loading/Unloading

        public static void PostSetupContent()
        {
            try
            {
                AchievementManager = new AchievementManagerProxy();

                _loadedAchievements = new Dictionary<Achievement, ModAchievement>();

                _localizedTextSetValue = typeof(LocalizedText).GetMethod("SetValue", PRIVATE_FIELD_BINDING_FLAGS);

                // Hooking
                #region UI Achievement Entry

                Type uiAchievementEntryType = typeof(UIAchievementListItem);

                _achievementIcon = uiAchievementEntryType.GetField(nameof(_achievementIcon), PRIVATE_FIELD_BINDING_FLAGS);
                _achievementIconBorders = uiAchievementEntryType.GetField(nameof(_achievementIconBorders), PRIVATE_FIELD_BINDING_FLAGS);
                _large = uiAchievementEntryType.GetField(nameof(_large), PRIVATE_FIELD_BINDING_FLAGS);

                _locked = uiAchievementEntryType.GetField(nameof(_locked), PRIVATE_FIELD_BINDING_FLAGS);
                _iconFrame = uiAchievementEntryType.GetField(nameof(_iconFrame), PRIVATE_FIELD_BINDING_FLAGS);
                _iconFrameLocked = uiAchievementEntryType.GetField(nameof(_iconFrameLocked), PRIVATE_FIELD_BINDING_FLAGS);
                _iconFrameUnlocked = uiAchievementEntryType.GetField(nameof(_iconFrameUnlocked), PRIVATE_FIELD_BINDING_FLAGS);

                #endregion

                On.Terraria.GameContent.UI.Elements.UIAchievementListItem.ctor += UIAchievementListItemOnCtor;


                // Loading
                ModAchievementLoader.Instance.TryLoad(); // Redundant, since singletons always load on call.
                AchievementManager.FakeLoad();
            }
            catch (Exception e)
            {
                throw new Exception(
                    "An error occured while initializing the achievements handler for Webmilio's Commons." +
                    "\nThis can happen when updating Webmilio's Commons between big version gaps. Restarting the game fixes the problem." +
                    "\nIt is recommended to simply exit the game without clicking continue (ALT+F4)." +
                    "\nBelow is for developers:\n\n", null);
            }
        }

        public static void Unload()
        {
            if (AchievementManager == null)
            {
                WebmilioCommonsMod.Instance.Logger.Error("Achievements getter was found to be null; canceling unload.");
                return;
            }

            AchievementManager.Dispose();

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

            bool added = true;

            if (AchievementManager.Achievements.ContainsKey(achievement.Name))
            {
                WebmilioCommonsMod.Instance.Logger.Info($"Achievement `{achievement.Name}` already exists; replacing with new.");
                AchievementManager.Achievements[achievement.Name] = achievement;

                added = false;
            }

            _loadedAchievements.Add(achievement, modAchievement);

            if (added)
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


        private static void UIAchievementListItemOnCtor(On.Terraria.GameContent.UI.Elements.UIAchievementListItem.orig_ctor orig, Terraria.GameContent.UI.Elements.UIAchievementListItem self, Achievement achievement, bool largeForOtherLanguages)
        {
            if (_letThrough != achievement.Name)
            {
                _letThrough = achievement.Name;
                orig(self, achievement, largeForOtherLanguages);
            }

            if (!_loadedAchievements.ContainsKey(achievement)) return;


            Rectangle originalFrame = (Rectangle)_iconFrame.GetValue(self);


            _iconFrameLocked.SetValue(self, new Rectangle(originalFrame.X + originalFrame.Width + 2, originalFrame.Y, originalFrame.Width, originalFrame.Height));
                
                
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

        #endregion


        private static UIImageFramed GetAchievementIcon(UIAchievementListItem achievementUIItem) => _achievementIcon.GetValue(achievementUIItem) as UIImageFramed;

        private static UIImage GetAchievementIconBorders(UIAchievementListItem achievementUIItem) => _achievementIconBorders.GetValue(achievementUIItem) as UIImage;


        public static AchievementManagerProxy AchievementManager { get; private set; }
    }
}
