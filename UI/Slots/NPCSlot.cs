using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace WebmilioCommons.UI.Slots;

public class NPCSlot : UIPanel
{
    public NPCSlot()
    {
        SetPadding(4);

        Append(Display = new()
        {
            Width = StyleDimension.Fill,
            Height = StyleDimension.Fill,

            IgnoresMouseInteraction = true
        });
    }

    public NPCSlot(int npcType, float scale = 1)
    {
        SetPadding(4);

        Append(Display = new(npcType, scale)
        {
            Width = StyleDimension.Fill,
            Height = StyleDimension.Fill,

            IgnoresMouseInteraction = true
        });
    }

    public void SetNPC(int npcType, float scale = 1)
    {
        Display.SetNPC(npcType, scale);
    }

    public NPCDisplay Display { get; }
    public NPC NPC => Display.NPC;
}