using Terraria.ModLoader;

namespace WebmilioCommons.Items
{
    public sealed class WCGlobalItem : GlobalItem
    {
        /*public override bool ConsumeItem(Item item, Player player) => item.type != ItemID.LifeCrystal || TryConsumeLifeCrystal(WCPlayer.Get(player));

        private bool TryConsumeLifeCrystal(WCPlayer wcPlayer)
        {
            if (WCPlayer.LifeCrystalLimiting && wcPlayer.HasReachedLifeCrystalLimit)
                return false;

            if (wcPlayer.LifeCrystalsConsumed < WCPlayer.LifeCrystalLimit)
                wcPlayer.LifeCrystalsConsumed++;

            return true;
        }*/
    }
}