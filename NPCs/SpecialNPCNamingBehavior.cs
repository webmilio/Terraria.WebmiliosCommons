using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Lang = On.Terraria.Lang;

namespace WebmilioCommons.NPCs
{
    public static class SpecialNPCNamingBehavior
    {
        private static Dictionary<int, Func<NPC, string>> _uniqueNPCNaming;
        private static Dictionary<int, LocalizedText> _localizationEntries;
        private static MethodInfo _localizedTextSetValue;


        internal static void Load()
        {
            _uniqueNPCNaming = new Dictionary<int, Func<NPC, string>>();
            _localizationEntries = new Dictionary<int, LocalizedText>();

            _localizedTextSetValue = typeof(LocalizedText).GetMethod("SetValue", BindingFlags.NonPublic | BindingFlags.Instance);

            Lang.GetNPCName += Lang_OnGetNPCName;
        }

        internal static void Unload()
        {
            _uniqueNPCNaming?.Clear();
            _uniqueNPCNaming = default;

            _localizationEntries?.Clear();
            _localizationEntries = default;

            _localizedTextSetValue = default;

            Lang.GetNPCName -= Lang_OnGetNPCName;
        }


        public static void RegisterUniqueNPCNaming(int npcId, Func<NPC, string> nameFunction)
        {
            _uniqueNPCNaming.Add(npcId, nameFunction);

            _localizationEntries.Add(npcId, default);
        }


        private static LocalizedText Lang_OnGetNPCName(Lang.orig_GetNPCName orig, int netId)
        {
            int npcType = NPCID.FromNetId(netId);

            if (!_uniqueNPCNaming.ContainsKey(netId))
                return orig(netId);

            string result = default;

            for (int i = 0; i < Main.npc.Length; i++)
                if (Main.npc[i].type == npcType)
                {
                    result = _uniqueNPCNaming[npcType](Main.npc[i]);
                    break;
                }

            if (result == default)
                return orig(netId);


            var localizedText = _localizationEntries[npcType];

            if (localizedText != default && localizedText.Key == result)
                return localizedText;

            _localizationEntries[npcType] = localizedText = CreateLocalizedText(result);
            return localizedText;
        }

        private static LocalizedText CreateLocalizedText(string value) => (LocalizedText)Activator.CreateInstance(typeof(LocalizedText), BindingFlags.NonPublic | BindingFlags.Instance, null, new object[] { value, value }, null);
    }
}