using Microsoft.Xna.Framework;
using Terraria;

namespace WebmilioCommons.Extensions
{
    public static class VectorExtensions
    {
        public static Vector2 VelocityVectorToMouse(this Vector2 start, float speed = 1.0f) => VelocityVector(start, Main.MouseWorld, speed);

        public static Vector2 VelocityVectorToMouse(this Vector2 start, float speed, Vector2 endOffset) => VelocityVector(start, Main.MouseWorld, speed, endOffset);

        public static Vector2 VelocityVector(this Vector2 start, Vector2 end, float speed = 1.0f) => (end - start).SafeNormalize(Vector2.UnitY) * speed;

        public static Vector2 VelocityVector(this Vector2 start, Vector2 end, float speed, Vector2 endOffset) => VelocityVector(start, end - endOffset, speed);
    }
}
