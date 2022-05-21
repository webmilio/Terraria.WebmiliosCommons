using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;
using WebmilioCommons.Players;
using WebmilioCommons.Tinq;

namespace WebmilioCommons.Proxies;

public class PlayersProxy : Proxy<ModPlayer>
{
    protected override IList<ModPlayer> GetSource()
    { 
        return (IList<ModPlayer>)typeof(PlayerLoader).GetField("players", NormalFieldFlags).GetValue(null);
    }

    public static void ForAllPlayers<V>(Action<V> action) where V : ModPlayer
    {
        ForAllPlayers<V>(player => player.GetModPlayer<V>(), action);
    }

    public static void ForAllPlayers<V>(Func<Player, V> getter, Action<V> action)
    {
        Main.player.DoActive(delegate (Player player)
        {
            var modPlayer = getter(player);
            action(modPlayer);
        });
    }

    public static bool PreCraftItem(Recipe recipe) => All<BetterModPlayer>(player => player.PreCraftItem(recipe));
    public static void CraftItem(Recipe recipe, Item item) => Do<BetterModPlayer>(player => player.CraftItem(recipe, item));
}