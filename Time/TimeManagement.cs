using System;
using System.Collections.Generic;
using System.Diagnostics;
using Terraria.ModLoader;
using WebmilioCommons.Extensions;

namespace WebmilioCommons.Time
{
    [Obsolete("Work-in-progress, feedback is highly appreciated on any crashes found.", false)]
    public static class TimeManagement
    {
        internal static List<string> bannedMods = new List<string>();

        private static List<int> 
            _npcs = new List<int>(),
            _items = new List<int>();


        #region Projectile Immunity

        #region NPC Immunity

        public static void AddNPCImmunity<T>() where T : ModNPC => AddNPCImmunity(ModContent.NPCType<T>());

        public static void AddNPCImmunity(int type)
        {
            ModNPC modNPC = ModContent.GetModNPC(type);

            if (modNPC != null && IsModBanned(modNPC.GetType()))
                return;

            _npcs.Add(type);
        }

        #endregion


        #region Projectile Immunity

        public static void AddProjectileImmunity<T>() where T : ModProjectile => AddProjectileImmunity(ModContent.ProjectileType<T>());

        public static void AddProjectileImmunity(int type)
        {
            ModProjectile modProjectile = ModContent.GetModProjectile(type);

            if (modProjectile != null && IsModBanned(modProjectile.GetType()))
                return;

            _projectiles.Add(type);
        }

        #endregion


        #region Immunity Banning

        public static void RequestImmunityModBan(Mod mod) => RequestImmunityModBan(mod.Name);

        public static void RequestImmunityModBan(string modName)
        {
            if (!bannedMods.Contains(modName))
                bannedMods.Add(modName);

            StackTrace stackTrace = new StackTrace();

            for (int i = 0; i )

            WebmilioCommonsMod.Instance.Logger.InfoFormat("Mod `{0}` requested mod {1} to be banned from requesting NPC immunity.", 
                null, modName);
        }

        private static bool IsModBanned<T>() => IsModBanned(typeof(T));
        private static bool IsModBanned(Type type) => bannedMods.Contains(type.GetModFromType().Name);
        #endregion

        #endregion


        internal static void Unload()
        {
            _npcs = null;
            _items = null;
        }
    }
}