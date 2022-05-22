using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using WebmilioCommons.Saving;

namespace WebmilioCommons.Players;

public abstract class ModPlayer : Terraria.ModLoader.ModPlayer
{
    public override void SaveData(TagCompound tag)
    {
        if (!PreSave(tag))
            return;

        ModContent.GetInstance<AutoSaveHandler>().Save(this, tag);
        ModSave(tag);
    }

    protected virtual bool PreSave(TagCompound tag) => true;
    protected virtual void ModSave(TagCompound tag) { }

    public override void LoadData(TagCompound tag)
    {
        if (!PreLoad(tag))
            return;

        ModContent.GetInstance<AutoSaveHandler>().Load(this, tag);
        ModLoad(tag);
    }

    protected virtual bool PreLoad(TagCompound tag) => true;
    protected virtual void ModLoad(TagCompound tag) { }

    /// <summary>Only called on the local client.</summary>
    /// <param name="recipe"></param>
    /// <returns><c>true</c> to continue with the item crafting; otherwise <c>false</c>.</returns>
    public virtual bool PreCraftItem(Recipe recipe) => true;
    public virtual void CraftItem(Recipe recipe, Item item) { }
}