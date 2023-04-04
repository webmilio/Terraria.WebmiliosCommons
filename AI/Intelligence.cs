using Terraria;
using Terraria.ModLoader;

namespace WebCom.AI;

public static class Intelligence
{
    public static IntelligenceBuilder.EntityBuilderCollection<NPC> Build(ModNPC npc)
    {
        return new IntelligenceBuilder.EntityBuilderCollection<NPC>(
            npc.NPC.GetGlobalNPC<IntelligenceNPC>());
    }

    public static IntelligenceBuilder.EntityBuilderCollection<Projectile> Build(ModProjectile projectile)
    {
        return new IntelligenceBuilder.EntityBuilderCollection<Projectile>(
            projectile.Projectile.GetGlobalProjectile<IntelligenceProjectile>());
    }
}