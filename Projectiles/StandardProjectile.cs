using Terraria.ModLoader;

namespace WebmilioCommons.Projectiles
{
    public abstract class StandardProjectile : ModProjectile
    {
        /// <summary></summary>
        /// <param name="frameCount"></param>
        /// <param name="frameCounterTime"></param>
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
