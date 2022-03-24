using Microsoft.Xna.Framework;

namespace WebmilioCommons.Extensions;

public static class RectangleExtensions
{
    public static Vector2 CenterSize(this Rectangle rectangle) => new(rectangle.Width / 2f, rectangle.Height / 2f);
}