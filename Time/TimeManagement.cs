using System;
using System.Collections.Generic;
using Terraria.ModLoader;
using WebmilioCommons.Extensions;

namespace WebmilioCommons.Time
{
    public static class TimeManagement
    {
        private static List<int> 
            _npcs = new List<int>(),
            _items = new List<int>();

        #region NPC Immunity

        public static void AddNPCImmunity<T>() where T : ModNPC => AddNPCImmunity(ModContent.NPCType<T>());

        public static void AddNPCImmunity(int type)
        {
            ModNPC modNPC = ModContent.GetModNPC(type);

            if (modNPC != null && IsTypeFromCalameme(modNPC.GetType()))
                return;

            _npcs.Add(type);
        }

        #endregion


        internal static void Unload()
        {
            _npcs = null;
            _items = null;
        }


        private static bool IsTypeFromCalameme<T>() => IsTypeFromCalameme(typeof(T));
        private static bool IsTypeFromCalameme(Type type) => type.GetModFromType().Name.Equals("CalamityMod", StringComparison.CurrentCultureIgnoreCase);
    }
}