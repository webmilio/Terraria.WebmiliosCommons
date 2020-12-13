using Terraria.DataStructures;
using Terraria.IO;
using WebmilioCommons.TileEntities;

namespace WebmilioCommons.Hooks.World
{
    internal static class WorldFileHooks
    {
        public static void Load()
        {
            WorldFile.OnWorldLoad += OnWorldLoad;
        }

        public static void Unload()
        {
            WorldFile.OnWorldLoad -= OnWorldLoad;
        }

        private static void OnWorldLoad()
        {
            foreach (var te in TileEntity.ByID.Values)
            {
                if (!(te is StandardTileEntity std))
                    continue;

                std.PostWorldLoad();
            }
        }
    }
}