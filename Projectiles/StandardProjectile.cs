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
            Projectile.frameCounter++;

            if (Projectile.frameCounter > frameCounterTime)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }

            if (Projectile.frame >= frameCount)
                Projectile.frame = 0;
        }


        public Player Owner
        {
            get
            {
                int owner = Projectile.owner;

                if (owner < 0 || owner > Main.maxPlayers)
                    return default;

                return Main.player[owner];
            }
        }


        public Vector2 Position
        {
            get => Projectile.position;
            set => Projectile.position = value;
        }

        public Vector2 Center
        {
            get => Projectile.Center;
            set => Projectile.Center = value;
        }


        public float Rotation
        {
            get => Projectile.rotation;
            set => Projectile.rotation = value;
        }

        public Vector2 Velocity
        {
            get => Projectile.velocity;
            set => Projectile.velocity = value;
        }


        public int Width
        {
            get => Projectile.width;
            set => Projectile.width = value;
        }

        public int Height
        {
            get => Projectile.height;
            set => Projectile.height = value;
        }


        public int Damage
        {
            get => Projectile.damage;
            set => Projectile.damage = value;
        }

        public int Penetrate
        {
            get => Projectile.penetrate;
            set => Projectile.penetrate = value;
        }


        public int TimeLeft
        {
            get => Projectile.timeLeft;
            set => Projectile.timeLeft = value;
        }


        public float AI0
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public float AI1
        {
            get => Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }


        public int Frame
        {
            get => Projectile.frame;
            set => Projectile.frame = value;
        }

        public int FrameCounter
        {
            get => Projectile.frameCounter;
            set => Projectile.frameCounter = value;
        }
    }
}
