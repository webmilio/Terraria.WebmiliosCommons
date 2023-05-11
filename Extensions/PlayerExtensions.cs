using Terraria;
using Terraria.DataStructures;

namespace WebCom.Extensions;

public static class PlayerExtensions
{
    /// <summary>This only exists to ensure uniformity in local player checks.</summary>
    /// <param name="player"></param>
    /// <returns><c>true</c> if the player is the local one; otherwise <c>false</c>.</returns>
    public static bool IsLocal(this Player player)
    {
        return player.whoAmI == Main.myPlayer;
    }

    /// <returns>The <see cref="Player"/> as an entity source.</returns>
    public static EntitySource_Parent AsEntitySource(this Player player)
    {
        return new EntitySource_Parent(player);
    }
}