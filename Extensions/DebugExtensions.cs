using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.UI;

namespace WebCom.Extensions;

public static class DebugExtensions
{
    private static readonly Color[] _colors;
    internal static int colorIndex = 0;

    static DebugExtensions()
    {
        var properties = typeof(Color).GetProperties(BindingFlags.Static | BindingFlags.Public);
        _colors = properties
            .Where(c => c.PropertyType == typeof(Color)).Select(c => (Color) c.GetValue(null))
            .Skip(1).ToArray();
    }

    public static void DrawBounds(this UIElement element) => DrawBounds(element.GetDimensions().ToRectangle(), Main.spriteBatch);
    public static void DrawBounds(this UIElement element, SpriteBatch spriteBatch) => DrawBounds(element.GetDimensions().ToRectangle(), spriteBatch);
    public static void DrawBounds(this Rectangle bounds) => DrawBounds(bounds, Main.spriteBatch);

    public static void DrawBounds(this Rectangle element, SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(TextureAssets.BlackTile.Value, element, _colors[colorIndex++]);
    }
}

internal class UIExtensionsSystem : ModSystem
{
    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
    {
        DebugExtensions.colorIndex = 0;
    }
}