using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria.ModLoader;

namespace WebmilioCommons.Identity
{
    public static class IdentityManager
    {
        private static List<Identity> _allIdentities;


        internal static void Load()
        {
            _allIdentities = new List<Identity>();

            foreach (Mod mod in ModLoader.Mods)
            {
                if (mod.Code == null)
                    continue;

                foreach (TypeInfo type in mod.Code.DefinedTypes.Where(t => t.IsSubclassOf(typeof(Identity)) && t.Assembly != Assembly.GetExecutingAssembly()))
                    _allIdentities.Add(Activator.CreateInstance(type) as Identity);
            }
        }

        internal static void PostSetupContent()
        {
            try
            {
                string unparsedSteamID64 = typeof(ModLoader).GetProperty("SteamID64", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null).ToString();

                if (!string.IsNullOrWhiteSpace(unparsedSteamID64))
                    SteamID64 = long.Parse(unparsedSteamID64);
            }
            catch (Exception)
            {
                //Console.WriteLine("Unable to fetch SteamID, assuming no steam is present.");
            }

            VerifyIdentities();
        }

        internal static void Unload()
        {

        }


        #region Steam

        private static void VerifyIdentities()
        {
            if (IsSteam)
                foreach (SteamIdentity identity in _allIdentities.Where(i => i is SteamIdentity))
                {
                    if (identity.Verify(SteamID64))
                    {
                        identity.Active = true;
                        return;
                    }
                }
        }


        public static Identity Identity { get; private set; }
        public static bool HasIdentity => Identity != null;

        public static long SteamID64 { get; private set; }
        public static bool HasSteamID64 => SteamID64 != 0;
        public static bool IsSteam => HasSteamID64 != false;

        #endregion
    }
}