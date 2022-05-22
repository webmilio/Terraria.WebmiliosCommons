using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using WebmilioCommons.Players;
using WebmilioCommons.Tinq;
using ModPlayer = WebmilioCommons.Players.ModPlayer;

namespace WebmilioCommons.Proxies;

public class PlayersProxy : Proxy<Terraria.ModLoader.ModPlayer>
{
    protected override IList<Terraria.ModLoader.ModPlayer> GetSource()
    { 
        return (IList<Terraria.ModLoader.ModPlayer>)typeof(PlayerLoader).GetField("players", NormalFieldFlags).GetValue(null);
    }

    public static void ForAllPlayers<V>(Action<V> action) where V : Terraria.ModLoader.ModPlayer
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

    public static bool PreCraftItem(Recipe recipe) => All<ModPlayer>(player => player.PreCraftItem(recipe));
    public static void CraftItem(Recipe recipe, Item item) => Do<ModPlayer>(player => player.CraftItem(recipe, item));
}