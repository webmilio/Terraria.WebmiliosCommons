using System;
using On.Terraria;
using Terraria.ModLoader;
using WebmilioCommons.Hooks.World;
using WorldHooks = WebmilioCommons.Hooks.World.WorldHooks;

namespace WebmilioCommons.Hooks.Wiring
{
    public class WireHooks
    {
        private delegate bool WorldGen_orig_PlaceKillWire_Any(int i, int j);


        internal static void Load()
        {
            WorldGen.PlaceWire += WorldGen_OnPlaceWire;
            WorldGen.PlaceWire2 += WorldGen_OnPlaceWire2;
            WorldGen.PlaceWire3 += WorldGen_OnPlaceWire3;
            WorldGen.PlaceWire4 += WorldGen_OnPlaceWire4;

            WorldGen.KillWire += WorldGen_OnKillWire;
            WorldGen.KillWire2 += WorldGen_OnKillWire2;
            WorldGen.KillWire3 += WorldGen_OnKillWire3;
            WorldGen.KillWire4 += WorldGen_OnKillWire4;
        }

        internal static void Unload()
        {
            WorldGen.KillWire4 -= WorldGen_OnKillWire4;
            WorldGen.KillWire3 -= WorldGen_OnKillWire3;
            WorldGen.KillWire2 -= WorldGen_OnKillWire2;
            WorldGen.KillWire -= WorldGen_OnKillWire;

            WorldGen.PlaceWire4 -= WorldGen_OnPlaceWire4;
            WorldGen.PlaceWire3 -= WorldGen_OnPlaceWire3;
            WorldGen.PlaceWire2 -= WorldGen_OnPlaceWire2;
            WorldGen.PlaceWire -= WorldGen_OnPlaceWire;
        }


        #region Place Wire

        private static bool WorldGen_OnPlaceWire(WorldGen.orig_PlaceWire orig, int i, int j) => WorldGen_OnPlaceWire_Any((x, y) => orig(x, y), WireColor.Red, i, j);
        private static bool WorldGen_OnPlaceWire2(WorldGen.orig_PlaceWire2 orig, int i, int j) => WorldGen_OnPlaceWire_Any((x, y) => orig(x, y), WireColor.Blue, i, j);
        private static bool WorldGen_OnPlaceWire3(WorldGen.orig_PlaceWire3 orig, int i, int j) => WorldGen_OnPlaceWire_Any((x, y) => orig(x, y), WireColor.Green, i, j);
        private static bool WorldGen_OnPlaceWire4(WorldGen.orig_PlaceWire4 orig, int i, int j) => WorldGen_OnPlaceWire_Any((x, y) => orig(x, y), WireColor.Yellow, i, j);

        private static bool WorldGen_OnPlaceWire_Any(WorldGen_orig_PlaceKillWire_Any orig, WireColor color, int i, int j)
        {
            bool result = WorldHooks.PrePlaceWire(color, i, j) && orig(i, j);

            if (result)
                WorldHooks.PostPlaceWire(color, i, j);

            return result;
        }

        #endregion

        #region Kill Wire

        private static bool WorldGen_OnKillWire(WorldGen.orig_KillWire orig, int i, int j) => WorldGen_OnKillWire_Any((x, y) => orig(x, y), WireColor.Red, i, j);
        private static bool WorldGen_OnKillWire2(WorldGen.orig_KillWire2 orig, int i, int j) => WorldGen_OnKillWire_Any((x, y) => orig(x, y), WireColor.Blue, i, j);
        private static bool WorldGen_OnKillWire3(WorldGen.orig_KillWire3 orig, int i, int j) => WorldGen_OnKillWire_Any((x, y) => orig(x, y), WireColor.Green, i, j);
        private static bool WorldGen_OnKillWire4(WorldGen.orig_KillWire4 orig, int i, int j) => WorldGen_OnKillWire_Any((x, y) => orig(x, y), WireColor.Yellow, i, j);

        private static bool WorldGen_OnKillWire_Any(WorldGen_orig_PlaceKillWire_Any orig, WireColor color, int i, int j)
        {
            bool result = WorldHooks.PreKillWire(color, i, j) && orig(i, j);

            if (result)
                WorldHooks.PostKillWire(color, i, j);

            return result;
        }

        #endregion
    }
}