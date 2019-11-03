using Terraria;

namespace WebmilioCommons.Time
{
    public interface IIsNPCImmune
    {
        bool IsImmune(NPC npc, TimeAlterationRequest request);
    }
}