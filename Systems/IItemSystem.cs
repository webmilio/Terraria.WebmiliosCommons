using Terraria;

namespace WebmilioCommons.Systems
{
    public interface IItemSystem
    {
        void OnHitNPC(Item item, Player player, NPC target, int damage, float knockBack, bool crit);
    }
}