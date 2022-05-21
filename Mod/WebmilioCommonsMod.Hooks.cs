using On.Terraria;
using WebmilioCommons.Helpers;
using WebmilioCommons.Items;
using WebmilioCommons.Proxies;
using Recipe = Terraria.Recipe;

namespace WebmilioCommons;

public partial class WebmilioCommonsMod
{
    private static void Main_OnCraftItem(Main.orig_CraftItem orig, Recipe recipe)
    {
        if (!PlayersProxy.PreCraftItem(recipe))
            return;

        orig(recipe);

        GlobalItemsProxy.Do<BetterGlobalItem>(g => g.OnItemCrafted(recipe, Terraria.Main.mouseItem));
        PlayersProxy.CraftItem(recipe, Terraria.Main.mouseItem);
    }

    private static void LoadHooks()
    {
        Main.CraftItem += Main_OnCraftItem;
    }

    private static void UnloadHooks()
    {
        Main.CraftItem -= Main_OnCraftItem;
    }
}
