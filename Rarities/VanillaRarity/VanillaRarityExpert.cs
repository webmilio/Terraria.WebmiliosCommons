using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace WebmilioCommons.Rarities.VanillaRarity
{
    public sealed class VanillaRarityExpert : ModRarity
    {
        public VanillaRarityExpert() : base(-12, -12)
        {
        }


        public override Color Color => new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB);
    }
}