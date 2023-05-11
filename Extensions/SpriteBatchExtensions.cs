using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using Terraria.GameContent;

namespace WebCom.Extensions;

public static class SpriteBatchExtensions
{
    public static void DrawLine(this SpriteBatch sb, int thickness, Vector2 start, Vector2 end, Color color)
    {
        Vector2 edge = end - start;
        float angle = (float)Math.Atan(edge.Y / edge.X);

        if (edge.X < 0)
        {
            angle = MathHelper.Pi + angle;
        }

        sb.Draw(TextureAssets.MagicPixel.Value,
            new Rectangle((int)start.X, (int)start.Y, (int)edge.Length(), thickness),
            null,
            color, angle, new Vector2(0, 500f),
            SpriteEffects.None, 0);
    }
}