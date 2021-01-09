using Terraria;

namespace WebmilioCommons.Time
{
    public interface IProjectileTimeImmune
    {
        bool IsImmune(Projectile projectile, TimeAlterationRequest request);
    }
}