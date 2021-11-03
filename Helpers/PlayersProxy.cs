using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;
using WebmilioCommons.Extensions;
using WebmilioCommons.Players;

namespace WebmilioCommons.Helpers;

public class PlayersProxy : ProxyHelper<ModPlayer>
{
    protected override IList<ModPlayer> GetOriginal()
    { 
        return (IList<ModPlayer>)typeof(PlayerLoader).GetField("players", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);
    }

    public static bool PreCraftItem(Recipe recipe) => All<BetterModPlayer>(player => player.PreCraftItem(recipe));
    public static void CraftItem(Recipe recipe, Item item) => Do<BetterModPlayer>(player => player.CraftItem(recipe, item));
}