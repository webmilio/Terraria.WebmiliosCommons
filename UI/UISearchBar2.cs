using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.Graphics;
using System;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.UI;
using WebCom.Extensions;

namespace WebCom.UI;

public class UISearchBar2 : UISearchBar
{
    public Asset<DynamicSpriteFont> fontNormal, fontBig;
    public bool ShowInputTicker { get; set; } = true;

    public UITextBox textbox;

    public UISearchBar2(Asset<DynamicSpriteFont> fontNormal, LocalizedText emptyContentText, float scale, Asset<DynamicSpriteFont> fontBig = null) : base(emptyContentText, scale)
    {
        fontBig ??= fontNormal;

        this.fontNormal = fontNormal;
        this.fontBig = fontBig;

        textbox = Children.First(e => e is UITextBox) as UITextBox;
    }

    protected override void DrawChildren(SpriteBatch spriteBatch)
    {
        var mMouseText = FontAssets.MouseText;
        var mDeathText = FontAssets.DeathText;

        FontAssets.MouseText = fontNormal;
        FontAssets.DeathText = fontBig;

        try
        {
            base.DrawChildren(spriteBatch);
        }
        finally
        {
            FontAssets.MouseText = mMouseText;
            FontAssets.DeathText = mDeathText;
        }
    }

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        base.DrawSelf(spriteBatch);

        textbox.ShowInputTicker = textbox.ShowInputTicker && ShowInputTicker;
    }
}
