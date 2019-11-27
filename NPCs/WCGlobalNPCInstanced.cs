using Terraria;
using Terraria.ModLoader;

namespace WebmilioCommons.NPCs
{
    public sealed partial class WCGlobalNPCInstanced : GlobalNPC
    {
        public override bool PreAI(NPC npc)
        {
            if (!PreAITime(npc))
                return false;

            return true;
        }


        public override void ModifyHitByItem(NPC npc, Player player, Item item, ref int damage, ref float knockback, ref bool crit) =>
            ModifyHitByPlayer(npc, player, player.direction, ref damage, ref knockback, ref crit);

        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection) =>
            ModifyHitByPlayer(npc, projectile.owner == 255 ? null : Main.player[projectile.owner], hitDirection, ref damage, ref knockback, ref crit);

        public void ModifyHitByPlayer(NPC npc, Player player, int hitDirection, ref int damage, ref float knockback, ref bool crit)
        {
            ModifyHitByPlayerTime(npc, player, hitDirection, ref damage, ref knockback, ref crit);
        }


        public override bool InstancePerEntity { get; } = true;
    }
}
