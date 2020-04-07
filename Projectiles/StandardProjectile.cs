using Terraria.ModLoader;

namespace WebmilioCommons.Projectiles
{
    public abstract class StandardProjectile : ModProjectile
    {
        public void StandardAnimateFrame(int frameCount, int frameCounterTime)
        {
            projectile.frameCounter++;

            if (projectile.frameCounter > frameCounterTime)
            {
                projectile.frame++;
                projectile.frameCounter = 0;
            }

            if (projectile.frame >= frameCount)
                projectile.frame = 0;
        }
    }
}
