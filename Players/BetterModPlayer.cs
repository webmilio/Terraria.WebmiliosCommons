using Terraria.ModLoader;

namespace WebmilioCommons.Players
{
    public abstract class BetterModPlayer : ModPlayer
    {
        public virtual bool CanInteractWithTownNPCs() => true;
    }
}