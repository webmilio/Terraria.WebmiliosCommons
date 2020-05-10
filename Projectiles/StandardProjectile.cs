using Microsoft.Xna.Framework;
using Terraria;
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


        public Player Owner
        {
            get
            {
                int owner = projectile.owner;

                if (owner < 0 || owner > Main.maxPlayers)
                    return default;

                return Main.player[owner];
            }
        }


        public Vector2 Position
        {
            get => projectile.position;
            set => projectile.position = value;
        }

        public Vector2 Center
        {
            get => projectile.Center;
            set => projectile.Center = value;
        }


        public float Rotation
        {
            get => projectile.rotation;
            set => projectile.rotation = value;
        }

        public Vector2 Velocity
        {
            get => projectile.velocity;
            set => projectile.velocity = value;
        }


        public int Width
        {
            get => projectile.width;
            set => projectile.width = value;
        }

        public int Height
        {
            get => projectile.height;
            set => projectile.height = value;
        }


        public int Damage
        {
            get => projectile.damage;
            set => projectile.damage = value;
        }

        public int Penetrate
        {
            get => projectile.penetrate;
            set => projectile.penetrate = value;
        }


        public int TimeLeft
        {
            get => projectile.timeLeft;
            set => projectile.timeLeft = value;
        }


        public float AI0
        {
            get => projectile.ai[0];
            set => projectile.ai[0] = value;
        }

        public float AI1
        {
            get => projectile.ai[1];
            set => projectile.ai[1] = value;
        }


        public int Frame
        {
            get => projectile.frame;
            set => projectile.frame = value;
        }

        public int FrameCounter
        {
            get => projectile.frameCounter;
            set => projectile.frameCounter = value;
        }
    }
}
