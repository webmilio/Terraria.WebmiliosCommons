using Terraria;
using Terraria.ModLoader;
using WebmilioCommons.States;
using WebmilioCommons.Time;

namespace WebmilioCommons.NPCs
{
    public sealed partial class WCGlobalNPCInstanced : GlobalNPC
    {
        private bool PreAITime(NPC npc)
        {
            if (CurrentRequest != TimeManagement.CurrentRequest)
                CurrentRequest = !TimeManagement.TimeAltered ? null : TimeManagement.CurrentRequest;

            if (CurrentRequest == null || !CurrentRequest.AlterNPCs || TimeManagement.IsNPCImmune(npc) || npc.modNPC is INPCTimeImmune nti && nti.IsImmune(npc, CurrentRequest))
            {
                if (State != null)
                {
                    State.Restore();
                    State = null;
                }

                TimeAltered = false;
                return true;
            }

            TimeAltered = true;

            if (TimeAltered && State == null)
                State = new NPCInstantState(npc);

            CanRunCurrentTick = CurrentRequest.TickRate != 0 && TimeManagement.CurrentTick % CurrentRequest.TickRate == 0;

            if (CanRunCurrentTick)
            {
                State.Restore();
                State = null;

                return true;
            }

            State.PreAI(npc);
            npc.frameCounter = 0;
            return false;
        }


        private void ModifyHitByPlayerTime(NPC npc, Player player, int hitDirection, ref int damage, ref float knockback, ref bool crit)
        {
            if (TimeAltered)
            {
                if (!CanRunCurrentTick)
                {
                    State.AccumulatedDamage += damage;
                    State.AccumulatedKnockback += knockback;
                    State.AccumulatedHitDirection = hitDirection;

                    damage = 0;
                    knockback = 0;
                }
            }
        }


        public TimeAlterationRequest CurrentRequest { get; private set; }

        public bool TimeAltered { get; private set; }
        public bool CanRunCurrentTick { get; private set; }

        public NPCInstantState State { get; private set; }
    }
}