using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using WebmilioCommons.Players;

namespace WebmilioCommons.Items
{
    public sealed class WCGlobalItem : GlobalItem
    {
        public override bool ConsumeItem(Item item, Player player) => item.type != ItemID.LifeCrystal || TryConsumeLifeCrystal(WCPlayer.Get(player));

        public override bool UseItem(Item item, Player player)
        {
            return true;
        }

        private bool TryConsumeLifeCrystal(WCPlayer wcPlayer)
        {
            if (WCPlayer.LifeCrystalLimiting && wcPlayer.HasReachedLifeCrystalLimit)
                return false;

            if (wcPlayer.LifeCrystalsConsumed < WCPlayer.LifeCrystalLimit)
                wcPlayer.LifeCrystalsConsumed++;

            return true;
        }
    }
}