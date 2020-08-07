using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using On.Terraria;
using WebmilioCommons.Hooks.Wiring;

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
            bool result = WorldHooksProxy.PreAddTrees();

            if (result)
            {
                orig();
                WorldHooksProxy.PostAddTrees();
            }
        }

        private static bool WorldGen_OnGrowTree(WorldGen.orig_GrowTree orig, int i, int j)
        {
            bool result = WorldHooksProxy.PreGrowTree(i, j) && orig(i, j);

            if (result)
                WorldHooksProxy.PostGrowTree(i, j);

            return result;
        }
    }
}
