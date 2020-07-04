using Terraria;
using WebmilioCommons.Players;

namespace WebmilioCommons.Items.Starting
{
    /// <summary>Implements a check to see if the player should start with the item.</summary>
    public interface IPlayerCanStartWith : IPlayerStartsWith
    {
        bool ShouldStartWith(WCPlayer wcPlayer, Player player, bool mediumcoreDeath);
    }
}