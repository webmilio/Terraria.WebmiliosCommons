using Terraria;
using Terraria.DataStructures;

namespace WebmilioCommons.States
{
    public class PlayerInstantState : EntityState<Player>
    {
        public PlayerInstantState(Player player) : base(player)
        {
            Life = player.statLife;
            Mana = player.statMana;
        }


        public override void Restore()
        {
            base.Restore();

            Entity.statLife = Life;
            Entity.statMana = Mana;

            if (AccumulatedDamage > 0)
                Entity.Hurt(PlayerDeathReason.ByCustomReason("From an unknown force"), AccumulatedDamage, AccumulatedHitDirection, true);
        }

        public override void PreAI(Player entity)
        {
            base.PreAI(entity);

            entity.statLife = Life;
            entity.statMana = Mana;
        }


        public int Life { get; set; }
        public int Mana { get; set; }
    }
}