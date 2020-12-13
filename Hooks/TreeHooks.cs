using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using On.Terraria;
using WebmilioCommons.Hooks.Wiring;
using WebmilioCommons.Hooks.World;

namespace WebmilioCommons.Hooks
{
    internal static class TreeHooks
    {
        internal static void Load()
        {
            WorldGen.AddTrees += WorldGen_OnAddTrees;
            WorldGen.GrowTree += WorldGen_OnGrowTree;
        }

        internal static void Unload()
        {
            WorldGen.GrowTree -= WorldGen_OnGrowTree;
            WorldGen.AddTrees -= WorldGen_OnAddTrees;
        }


        private static void WorldGen_OnAddTrees(WorldGen.orig_AddTrees orig)
        {
            bool result = WorldHooks.PreAddTrees();

            if (result)
            {
                orig();
                WorldHooks.PostAddTrees();
            }
        }

        private static bool WorldGen_OnGrowTree(WorldGen.orig_GrowTree orig, int i, int j)
        {
            bool result = WorldHooks.PreGrowTree(i, j) && orig(i, j);

            if (result)
                WorldHooks.PostGrowTree(i, j);

            return result;
        }
    }
}
