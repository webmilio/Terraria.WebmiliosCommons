using Terraria;
using WebmilioCommons.Players;

namespace WebmilioCommons.Items
{
    public interface ICanBeSold
    {
        bool CanBeSold(WCPlayer wcPlayer, NPC vendor, Item[] shopInventory);
    }
}