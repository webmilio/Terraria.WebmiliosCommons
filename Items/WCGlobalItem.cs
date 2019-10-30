using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using WebmilioCommons.Players;

namespace WebmilioCommons.Items
{
    public class WCGlobalItem : GlobalItem
    {
        public override bool UseItem(Item item, Player player)
        {
            WCPlayer wcPlayer = WCPlayer.Get(player);

            if (item.type == ItemID.LifeCrystal)
                return TryConsumeLifeCrystal(wcPlayer);

            return true;
        }

        private bool TryConsumeLifeCrystal(WCPlayer wcPlayer)
        {
            if (WCPlayer.LifeCrystalLimiting && wcPlayer.LifeCrystalsConsumed == 15)
                return false;

            if (wcPlayer.LifeCrystalsConsumed < 15)
                wcPlayer.LifeCrystalsConsumed++;

            return true;
        }
    }
}