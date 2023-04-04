using Terraria;
using Terraria.ModLoader;

namespace WebCom.AI;

public interface IIntelligenceGlobal<T>
{
    public IntelligenceBuilder.EntityBuilderProvider<T> Intelligence { get; set; }
}

internal class IntelligenceNPC : GlobalNPC, IIntelligenceGlobal<NPC>
{
    public override bool PreAI(NPC npc)
    {
        return Intelligence?.PreAI() ?? true;
    }

    public override void AI(NPC npc)
    {
        Intelligence?.AI();
    }

    public override void PostAI(NPC npc)
    {
        Intelligence?.PostAI();
    }

    public IntelligenceBuilder.EntityBuilderProvider<NPC> Intelligence { get; set; }

    public override bool InstancePerEntity => true;
}

internal class IntelligenceProjectile : GlobalProjectile, IIntelligenceGlobal<Projectile>
{
    public override bool PreAI(Projectile projectile)
    {
        return Intelligence?.PreAI() ?? true;
    }

    public override void AI(Projectile projectile)
    {
        Intelligence?.AI();
    }

    public override void PostAI(Projectile projectile)
    {
        Intelligence?.PostAI();
    }

    public IntelligenceBuilder.EntityBuilderProvider<Projectile> Intelligence { get; set; }

    public override bool InstancePerEntity => true;
}