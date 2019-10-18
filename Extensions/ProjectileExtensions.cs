using Microsoft.Xna.Framework;
using Terraria;

namespace WebmilioCommons.Extensions
{
    public static class ProjectileExtensions
    {
        public static Vector2 GetProjectileScreenPosition(this Projectile projectile) => projectile.position - Main.screenPosition;


        public static int GetId(this Projectile projectile)
        {
            for (int i = 0; i < Main.projectile.Length; i++)
                if (Main.projectile[i] == projectile)
                    return i;

            return -1;
        }
    }
}
