using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using WebmilioCommons.Worlds;

namespace WebmilioCommons.Hooks.Wiring
{
    public static class WorldHooksProxy
    {
        private static IList<ModWorld> _worlds;
        private static List<BetterModWorld> _betterWorlds;


        internal static void Load()
        {
            try
            {
                _worlds = (IList<ModWorld>) typeof(WorldHooks).GetField("worlds", BindingFlags.Static | BindingFlags.NonPublic).GetValue(default);
            }
            catch
            {
                WebmilioCommonsMod.Instance.Logger.ErrorFormat("Error while hooking into the list of {0}: certain functionalities will be unavailable.", nameof(ModWorld));
            }
        }

        internal static void PostSetupContent()
        {
            _betterWorlds = new List<BetterModWorld>();

            for (int i = 0; i < _worlds.Count; i++)
                if (_worlds[i] is BetterModWorld bmw)
                    _betterWorlds.Add(bmw);

            if (_betterWorlds.Count <= 0)
                return;

            TreeHooks.Unload();
            WireHooks.Load();
        }

        internal static void Unload()
        {
            WireHooks.Unload();
            TreeHooks.Unload();

            _worlds = default;

            _betterWorlds?.Clear();
            _betterWorlds = default;
        }


        #region Wiring

        public static bool PrePlaceWire(WireColor color, int i, int j)
        {
            bool
                giveWireBack = true,
                result = ForAllBetter(bmw => bmw.PrePlaceWire(color, i, j, ref giveWireBack));

            if (!result && giveWireBack)
                Item.NewItem(new Vector2(i, j), ItemID.Wire);

            return result;
        }

        public static void PostPlaceWire(WireColor color, int i, int j) => ForAllBetter(bmw => bmw.PostPlaceWire(color, i, j));


        public static bool PreKillWire(WireColor color, int i, int j) => ForAllBetter(bmw => bmw.PreKillWire(color, i, j));

        public static void PostKillWire(WireColor color, int i, int j) => ForAllBetter(bmw => bmw.PostKillWire(color, i, j));

        #endregion


        #region Trees

        public static bool PreAddTrees() => ForAllBetter(bmw => bmw.PreAddTrees());

        public static void PostAddTrees() => ForAllBetter(bmw => bmw.PostAddTrees());


        public static bool PreGrowTree(int i, int j) => ForAllBetter(bmw => bmw.PreGrowTree(i, j));

        public static void PostGrowTree(int i, int j) => ForAllBetter(bmw => bmw.PostGrowTree(i, j));

        #endregion


        private static bool ForAllBetter(Predicate<BetterModWorld> check)
        {
            for (int i = 0; i < _betterWorlds.Count; i++)
                if (!check(_betterWorlds[i]))
                    return false;

            return true;
        }

        private static void ForAllBetter(Action<BetterModWorld> action)
        {
            for (int i = 0; i < _betterWorlds.Count; i++)
                action(_betterWorlds[i]);
        }


        public static ModWorld Get(int index) => _worlds[index];
        public static BetterModWorld GetBetter(int index) => _betterWorlds[index];


        public static int ModWorldCount => _worlds.Count;

        public static int BetterModWorldCount => _betterWorlds.Count;
    }
}