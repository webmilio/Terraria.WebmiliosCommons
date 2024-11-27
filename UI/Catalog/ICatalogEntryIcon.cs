using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.ID;

namespace WebCom.UI.Catalog;

public interface ICatalogEntryIcon
{
    public void Update(ICatalogEntry entry, Rectangle hitbox, EntryIconDrawSettings settings);
    public void Draw(ICatalogEntry entry, SpriteBatch spriteBatch, EntryIconDrawSettings settings);

    public bool IsUnlocked(ICatalogEntry entry);
    public string GetHoverText(ICatalogEntry entry);

    public ICatalogEntryIcon CreateClone();
}

public class CatalogEntryIcon : ICatalogEntryIcon
{
    public Asset<Texture2D> Icon { get; init; }

    public void Update(ICatalogEntry entry, Rectangle hitbox, EntryIconDrawSettings settings)
    {
        ; // We don't do anything here.
    }

    public void Draw(ICatalogEntry entry, SpriteBatch spriteBatch, EntryIconDrawSettings settings)
    {
        var center = settings.iconbox.Center;
        var texture = Icon.Value;

        spriteBatch.Draw(texture, center.ToVector2(), texture.Bounds, Color.White, 
            0, texture.Bounds.Center(), 1, SpriteEffects.None, 0);
        //spriteBatch.Draw(texture.Value, settings.iconbox, Color.White);
    }

    public bool IsUnlocked(ICatalogEntry entry)
    {
        throw new System.NotImplementedException();
    }

    public string GetHoverText(ICatalogEntry entry)
    {
        return "CHANGE ME!";
    }

    public ICatalogEntryIcon CreateClone()
    {
        throw new System.NotImplementedException();
    }
}