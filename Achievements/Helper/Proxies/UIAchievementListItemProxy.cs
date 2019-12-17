using System;
using System.Reflection;
using Terraria.GameContent.UI.Elements;

namespace WebmilioCommons.Achievements.Helper.Proxies
{
    public sealed class UIAchievementListItemProxy : IDisposable
    {
        private static FieldInfo _achievementIcon, _achievementIconBorders, _large, _locked, _iconFrame, _iconFrameLocked, _iconFrameUnlocked;


        public UIAchievementListItemProxy()
        {
            Type uiAchievementEntryType = typeof(UIAchievementListItem);

            _achievementIcon = uiAchievementEntryType.GetField(nameof(_achievementIcon), ModAchievementHelper.PRIVATE_FIELD_BINDING_FLAGS);
            _achievementIconBorders = uiAchievementEntryType.GetField(nameof(_achievementIconBorders), ModAchievementHelper.PRIVATE_FIELD_BINDING_FLAGS);
            _large = uiAchievementEntryType.GetField(nameof(_large), ModAchievementHelper.PRIVATE_FIELD_BINDING_FLAGS);

            _locked = uiAchievementEntryType.GetField(nameof(_locked), ModAchievementHelper.PRIVATE_FIELD_BINDING_FLAGS);
            _iconFrame = uiAchievementEntryType.GetField(nameof(_iconFrame), ModAchievementHelper.PRIVATE_FIELD_BINDING_FLAGS);
            _iconFrameLocked = uiAchievementEntryType.GetField(nameof(_iconFrameLocked), ModAchievementHelper.PRIVATE_FIELD_BINDING_FLAGS);
            _iconFrameUnlocked = uiAchievementEntryType.GetField(nameof(_iconFrameUnlocked), ModAchievementHelper.PRIVATE_FIELD_BINDING_FLAGS);
        }





        public void Dispose()
        {
            
        }
    }
}