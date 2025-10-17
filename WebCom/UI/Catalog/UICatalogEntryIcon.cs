using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.UI;

namespace WebCom.UI.Catalog;

public class UICatalogEntryIcon : UIElement
{
    public ICatalogEntry Entry { get; }

    private Asset<Texture2D> _lockedTexture;
    private bool _portrait;
    
    public bool forceHover;

    public UICatalogEntryIcon(ICatalogEntry entry, bool portrait)
    {
        Entry = entry;
        _portrait = portrait;

        OverrideSamplerState = Main.DefaultSamplerState;

        UseImmediateMode = true;

        // TODO: Make this changeable by the creator.
        _lockedTexture = Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Icon_Locked");
    }

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        base.DrawSelf(spriteBatch);

        var dimensions = GetDimensions();
        
        bool unlocked = true; // Change this to check for unlock state.
        bool hovered = IsMouseHovering || forceHover;

        if (unlocked)
        {
            Entry.Icon.Draw(Entry, spriteBatch, new EntryIconDrawSettings
            {
                iconbox = dimensions.ToRectangle(),

                IsPortrait = _portrait,
                IsHovered = hovered
            });
        }
        else
        {
            spriteBatch.Draw(_lockedTexture.Value, dimensions.Center(), null, Color.White * .15f, 0,
                _lockedTexture.Value.Size() / 2, 1, SpriteEffects.None, 0);
        }
    }
}
