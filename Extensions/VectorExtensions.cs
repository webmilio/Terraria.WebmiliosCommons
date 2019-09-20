using Microsoft.Xna.Framework;
using Terraria;

namespace WebmilioCommons.Extensions
{
    public static class VectorExtensions
    {
        public static Vector2 VelocityVectorToMouse(Vector2 start, float speed = 1.0f) => VelocityVector(Main.MouseWorld, start, speed);

        public static Vector2 VelocityVector(Vector2 start, Vector2 end, float speed = 1.0f) => (end - start).SafeNormalize(-Vector2.UnitY) * speed;
    }
}
