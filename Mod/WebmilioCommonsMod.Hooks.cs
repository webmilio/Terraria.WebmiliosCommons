using On.Terraria;
using WebmilioCommons.Helpers;
using WebmilioCommons.Players;
using Recipe = Terraria.Recipe;

namespace WebmilioCommons;

public partial class WebmilioCommonsMod
{
    private void Main_OnCraftItem(Main.orig_CraftItem orig, Recipe r)
    {
        if (!PlayerHelpers.All<BetterModPlayer>(player => player.PreCraftItem(r)))
            return;

        orig(r);
        PlayerHelpers.ForModPlayers<BetterModPlayer>(player => player.CraftItem(r, Terraria.Main.mouseItem));
    }

    private void LoadHooks()
    {
        Main.CraftItem += Main_OnCraftItem;
    }

    private void UnloadHooks()
    {
        Main.CraftItem -= Main_OnCraftItem;
    }
}
