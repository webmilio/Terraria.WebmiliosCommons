using On.Terraria;
using Recipe = Terraria.Recipe;

namespace WebmilioCommons;

public partial class WebmilioCommonsMod
{
    private void Main_OnCraftItem(Main.orig_CraftItem orig, Recipe r)
    {
        orig(r);

        
    }

    private void LoadHooks()
    {
        Main.CraftItem += Main_OnCraftItem;
    }

    private void UnloadHooks()
    {

    }
}
