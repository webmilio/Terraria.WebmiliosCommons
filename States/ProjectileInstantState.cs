using Terraria;

namespace WebmilioCommons.States
{
    public class ProjectileInstantState : EntityState<Projectile>
    {
        public ProjectileInstantState(Projectile projectile) : base(projectile)
        {
            Damage = projectile.damage;

            TimeLeft = projectile.timeLeft;

            AI = projectile.ai;
            AIStyle = projectile.aiStyle;

            Frame = projectile.frame;
            FrameCounter = projectile.frameCounter;
        }


        public override void Restore()
        {
            base.Restore();

            Entity.damage = Damage;

            Entity.timeLeft = TimeLeft;

            Entity.ai = AI;
            Entity.aiStyle = AIStyle;
        }

        public override void PreAI(Projectile entity)
        {
            base.PreAI(entity);

            entity.damage = 0;

            entity.frameCounter = FrameCounter;
            entity.frame = Frame;

            entity.timeLeft = TimeLeft;
        }


        public int Damage { get; set; }

        public int TimeLeft { get; set; }

        public float[] AI { get; set; }
        public int AIStyle { get; set; }

        public int Frame { get; set; }
        public int FrameCounter { get; set; }
    }
}