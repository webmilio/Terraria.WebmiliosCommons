using Terraria;
using Terraria.ModLoader;
using WebmilioCommons.States;
using WebmilioCommons.Time;

namespace WebmilioCommons.NPCs
{
    public sealed class TimeManagementNPC : GlobalNPC
    {
        public override bool PreAI(NPC npc)
        {
            if (CurrentRequest != TimeManagement.CurrentRequest)
                CurrentRequest = !TimeManagement.TimeAltered ? null : TimeManagement.CurrentRequest;

            if (CurrentRequest == null || !CurrentRequest.AlterNPCs || TimeManagement.IsNPCImmune(npc))
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


        public override void ModifyHitByItem(NPC npc, Player player, Item item, ref int damage, ref float knockback, ref bool crit) =>
            ModifyHitByPlayer(npc, player, player.direction, ref damage, ref knockback, ref crit);

        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection) =>
            ModifyHitByPlayer(npc, projectile.owner == 255 ? null : Main.player[projectile.owner], hitDirection, ref damage, ref knockback, ref crit);

        public void ModifyHitByPlayer(NPC npc, Player player, int hitDirection, ref int damage, ref float knockback, ref bool crit)
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

        public override bool InstancePerEntity => true;
    }
}