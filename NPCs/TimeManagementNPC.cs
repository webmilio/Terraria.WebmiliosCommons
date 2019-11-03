using Terraria;
using Terraria.ModLoader;
using WebmilioCommons.Time;

namespace WebmilioCommons.NPCs
{
    public sealed class TimeManagementNPC : GlobalNPC
    {
        public override bool PreAI(NPC npc)
        {
            if (TimeManagement.IsNPCImmune(npc))
                return true;

            IsTimeAltered = TimeManagement.TimeAltered;

            if (IsTimeAltered)
            {
                if (!TimeManagement.npcStates.ContainsKey(npc))
                    TimeManagement.RegisterStoppedNPC(npc);

                TimeManagement.npcStates[npc].PreAI(npc);
                npc.frameCounter = 0;
                return false;
            }

            return true;
        }


        public override void ModifyHitByItem(NPC npc, Player player, Item item, ref int damage, ref float knockback, ref bool crit) =>
            ModifyHitByPlayer(npc, player, player.direction, ref damage, ref knockback, ref crit);

        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection) =>
            ModifyHitByPlayer(npc, projectile.owner == 255 ? null : Main.player[projectile.owner], hitDirection, ref damage, ref knockback, ref crit);

        public void ModifyHitByPlayer(NPC npc, Player player, int hitDirection, ref int damage, ref float knockback, ref bool crit)
        {
            if (IsTimeAltered && TimeManagement.TimeStopped)
            {
                NPCInstantState state = TimeManagement.npcStates[npc];

                state.AccumulatedDamage += damage;
                state.AccumulatedKnockback += knockback;
                state.AccumulatedHitDirection = hitDirection;

                damage = 0;
                knockback = 0;
            }
        }


        public bool IsTimeAltered { get; private set; }

        public override bool InstancePerEntity => true;
    }
}