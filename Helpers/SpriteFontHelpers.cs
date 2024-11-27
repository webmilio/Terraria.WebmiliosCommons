using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;

namespace WebCom.Helpers;

public class SpriteFontHelpers
{
    public static void DrawBorderString(SpriteBatch sb, DynamicSpriteFont font, string text, Vector2 pos, Color textColor, Color borderColor, Vector2 origin = default, float scale = 1f)
    {
        float x = pos.X;
        float y = pos.Y;

        var color = borderColor;
        var zero = Vector2.Zero;

        for (int i = 0; i < 5; i++)
        {
            switch (i)
            {
                case 0:
                    zero.X = x - 2f;
                    zero.Y = y;
                    break;
                case 1:
                    zero.X = x + 2f;
                    zero.Y = y;
                    break;
                case 2:
                    zero.X = x;
                    zero.Y = y - 2f;
                    break;
                case 3:
                    zero.X = x;
                    zero.Y = y + 2f;
                    break;
                default:
                    zero.X = x;
                    zero.Y = y;
                    color = textColor;
                    break;
            }

            sb.DrawString(font, text, zero, color, 0f, origin, scale, SpriteEffects.None, 0f);
        }
    }
}
