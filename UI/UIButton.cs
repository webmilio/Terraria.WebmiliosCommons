using Microsoft.Xna.Framework;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.UI;

namespace WebmilioCommons.UI;

public class UIButton : UIPanel
{
    private Color? _originalColor;

    public UIButton(string text, float textScale = 1, bool large = false)
    {
        Label = new(text, textScale, large);
        SetDefaults(Label);

        Append(Label);
    }

    public UIButton(LocalizedText text, float textScale = 1, bool large = false)
    {
        Label = new(text, textScale, large);
        SetDefaults(Label);

        Append(Label);
    }

    private static void SetDefaults(UIElement label)
    {
        label.VAlign = label.HAlign = .5f;
        label.Width = StyleDimension.Fill;

        label.IgnoresMouseInteraction = true;
    }

    public override void MouseOver(UIMouseEvent evt)
    {
        if (_originalColor == null)
        {
            _originalColor = BackgroundColor;
        }

        BackgroundColor = ShiftColor(BackgroundColor, 25);
        base.MouseOver(evt);
    }

    public override void MouseOut(UIMouseEvent evt)
    {
        if (_originalColor != null)
        {
            BackgroundColor = _originalColor.Value;
            _originalColor = null;
        }

        base.MouseOut(evt);
    }

    public override void MouseDown(UIMouseEvent evt)
    {
        BackgroundColor = ShiftColor(BackgroundColor, -50);
        base.MouseDown(evt);
    }

    public override void MouseUp(UIMouseEvent evt)
    {
        if (_originalColor != null)
        {
            BackgroundColor = ShiftColor(BackgroundColor, 50);
        }

        base.MouseUp(evt);
    }

    private static Color ShiftColor(Color color, sbyte shift)
    {
        return new Color(color.R + shift, color.G + shift, color.B + shift);
    }

    public UIText Label { get; }
}