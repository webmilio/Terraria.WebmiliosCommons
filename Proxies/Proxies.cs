using Terraria;
using Terraria.ID;
using WebmilioCommons.Achievements.Helper;
using WebmilioCommons.Proxies.Players;

namespace WebmilioCommons.Proxies
{
    internal static class Proxies
    {
        internal static void Load()
        {
            ModInternalsProxy.Load();
            PlayerHooksProxy.Load();
        }

        internal static void PostSetupContent()
        {
            if (Main.netMode != NetmodeID.Server)
            {
                ModAchievementHelper.PostSetupContent();
            }
        }

        internal static void Unload()
        {
            if (Main.netMode != NetmodeID.Server)
            {
                ModAchievementHelper.Unload();
            }


            PlayerHooksProxy.Unload();
            ModInternalsProxy.Unload();
        }

        internal static void WorldGen_OnSaveAndQuit()
        {
            PlayerHooksProxy.ClearModPlayer();
        }
    }
}