using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.UI;

namespace WebCom.UI.Catalog;

public class UICatalogEntryButton : UIElement
{
    public ICatalogEntry Entry { get; }

    private UIImage _bordersGlow, _bordersOverlay, _borders;
    private UICatalogEntryIcon _icon;

    public UICatalogEntryButton(ICatalogEntry entry, bool prettyPortrait)
    {
        Entry = entry;

        SetPadding(0);

        var slot = new UIElement
        {
            Width = StyleDimension.FromPixelsAndPercent(-4, 1),
            Height = StyleDimension.FromPixelsAndPercent(-4, 1),

            IgnoresMouseInteraction = true,
            OverflowHidden = true,

            HAlign = .5f,
            VAlign = .5f
        };

        slot.SetPadding(0);

        // TODO: Change all static textures to be customizable.
        var backImage = new UIImage(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Slot_Back"))
        {
            VAlign = .5f,
            HAlign = .5f
        };
        slot.Append(backImage);

        // TODO: Add pretty portrait
        _icon = new UICatalogEntryIcon(entry, prettyPortrait)
        {
            Width = StyleDimension.Fill,
            Height = StyleDimension.Fill,

            IgnoresMouseInteraction = true
        };

        slot.Append(_icon);
        Append(slot);

        _bordersGlow = new UIImage(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Slot_Selection"))
        {
            VAlign = .5f,
            HAlign = .5f,

            IgnoresMouseInteraction = true
        };

        _bordersOverlay = new UIImage(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Slot_Overlay"))
        {
            Color = Color.White * 0.6f,

            VAlign = 0.5f,
            HAlign = 0.5f,

            IgnoresMouseInteraction = true,
        };
        Append(_bordersOverlay);

        _borders = new UIImage(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Slot_Front"))
        {
            VAlign = .5f,
            HAlign = .5f,

            IgnoresMouseInteraction = true
        };

        Append(_borders);

        if (prettyPortrait)
        {
            RemoveChild(_bordersOverlay);
        }

        // TODO: Finish adding pretty portrait stuff I don't really care
    }

    public override void MouseOver(UIMouseEvent evt)
    {
        base.MouseOver(evt);

        SoundEngine.PlaySound(SoundID.MenuTick);

        RemoveChild(_borders);
        RemoveChild(_bordersGlow);
        RemoveChild(_bordersOverlay);

        Append(_borders);
        Append(_bordersGlow);

        _icon.forceHover = true;
    }

    public override void MouseOut(UIMouseEvent evt)
    {
        base.MouseOut(evt);

        RemoveChild(_borders);
        RemoveChild(_bordersGlow);
        RemoveChild(_bordersOverlay);

        Append(_bordersOverlay);
        Append(_borders);

        _icon.forceHover = false;
    }
}
