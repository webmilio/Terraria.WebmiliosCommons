using Terraria;
using Terraria.ModLoader;

namespace WebmilioCommons.Projectiles
{
    public sealed partial class WCGlobalProjectileInstanced : GlobalProjectile
    {
        public override bool PreAI(Projectile projectile)
        {
            if (!PreAITime(projectile))
                return false;

            return true;
        }


        public override bool InstancePerEntity { get; } = true;
    }
}
