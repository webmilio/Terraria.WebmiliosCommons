using Terraria;

namespace WebmilioCommons.States
{
    public class NPCInstantState : EntityState<NPC>
    {
        public NPCInstantState(NPC npc) : base(npc)
        {
            Damage = npc.damage;
            Life = npc.life;

            AI = npc.ai;
            NoGravity = npc.noGravity;
            FrameCounter = npc.frameCounter;
        }


        public override void Restore()
        {
            base.Restore();

            Entity.damage = Damage;
            Entity.life = Life;
            
            if (AccumulatedDamage > 0)
            {
                float oldKBR = Entity.knockBackResist;

                Entity.knockBackResist = 1;
                Entity.StrikeNPC(AccumulatedDamage, AccumulatedKnockback, AccumulatedHitDirection);

                Entity.knockBackResist = oldKBR;
            }

            Entity.ai = AI;
            Entity.noGravity = NoGravity;
            Entity.frameCounter = FrameCounter;
        }

        public override void PreAI(NPC entity)
        {
            base.PreAI(entity);

            entity.damage = 0;
            entity.life = Life;
            

            entity.frameCounter = FrameCounter;
            entity.noGravity = true;
            entity.ai = AI;
        }


        public int Damage { get; set; }
        public int Life { get; set; }

        public float[] AI { get; set; }
        public bool NoGravity { get; set; }
        public double FrameCounter { get; set; }
    }
}