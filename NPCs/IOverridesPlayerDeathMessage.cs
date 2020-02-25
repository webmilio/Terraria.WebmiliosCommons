using Terraria;
using Terraria.DataStructures;
using WebmilioCommons.Players;

namespace WebmilioCommons.NPCs
{
    /// <summary>Allows you to implement custom logic intended to override the death message of a player when killed by an entity implementing this interface.</summary>
    public interface IOverridesPlayerDeathMessage
    {
        /// <summary>Allows you to override the death message for a player when killed by the entity implementing this method.</summary>
        /// <param name="player"></param>
        /// <param name="damage"></param>
        /// <param name="hitDirection"></param>
        /// <param name="pvp"></param>
        /// <param name="damageSource"></param>
        /// <returns>The custom death message.</returns>
        string GetDeathMessage(Player player, double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource);
    }
}