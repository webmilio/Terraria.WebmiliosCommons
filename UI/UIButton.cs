using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.UI;
using WebmilioCommons.Commons;

namespace WebmilioCommons.UI;

public class UIButton : UIPanel, IHaveTags
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

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        if (_lastEnabled != Enabled)
        {
            if (_lastEnabled && !Enabled)
            {
                BackgroundColor = ShiftColor(BackgroundColor, -50);
            }
            else if (!_lastEnabled && Enabled)
            {
                BackgroundColor = ShiftColor(BackgroundColor, 50);
            }
        }

        _lastEnabled = Enabled;
        base.DrawSelf(spriteBatch);
    }

    public override void Click(UIMouseEvent evt)
    {
        if (!Enabled)
            return;

        base.Click(evt);
    }

    public override void MouseOver(UIMouseEvent evt)
    {
        if (!Enabled)
            return;

        if (_originalColor == null)
        {
            _originalColor = BackgroundColor;
        }

        BackgroundColor = ShiftColor(BackgroundColor, 25);
        base.MouseOver(evt);
    }

    public override void MouseOut(UIMouseEvent evt)
    {
        if (!Enabled)
            return;

        if (_originalColor != null)
        {
            BackgroundColor = _originalColor.Value;
            _originalColor = null;
        }

        base.MouseOut(evt);
    }

    public override void MouseDown(UIMouseEvent evt)
    {
        if (!Enabled)
            return;

        BackgroundColor = ShiftColor(BackgroundColor, -50);
        base.MouseDown(evt);
    }

    public override void MouseUp(UIMouseEvent evt)
    {
        if (!Enabled)
            return;

        if (_originalColor != null)
        {
            BackgroundColor = ShiftColor(BackgroundColor, 50);
        }

        base.MouseUp(evt);
    }

    protected static Color ShiftColor(Color color, sbyte shift)
    {
        return new Color(color.R + shift, color.G + shift, color.B + shift);
    }

    private bool _lastEnabled = true;
    public bool Enabled { get; set; } = true;

    public UIText Label { get; }

    public Dictionary<string, object> Tags { get; init; }
}