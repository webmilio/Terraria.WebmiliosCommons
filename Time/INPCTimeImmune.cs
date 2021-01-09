using Terraria;

namespace WebmilioCommons.Time
{
    public interface INPCTimeImmune
    {
        bool IsImmune(NPC npc, TimeAlterationRequest request);
    }
}