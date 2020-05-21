using System;
using Microsoft.Xna.Framework;

namespace WebmilioCommons.Rarities
{
    [Obsolete("WIP, doesn't do anything (but sometimes does, so you can still prepare your rarities in advance)")]
    public abstract class ModRarity
    {
        /// <summary>Called when loading all <see cref="ModRarity"/>.</summary>
        /// <param name="lowerVanillaRarity">Fallback vanilla color in case the <see cref="ModRarityLoader"/> fails to load.</param>
        /// <param name="upperVanillaRarity"></param>
        /// <param name="color"></param>
        protected ModRarity(int lowerVanillaRarity, int upperVanillaRarity, Color color = default)
        {
            LowerVanillaRarity = lowerVanillaRarity;
            UpperVanillaRarity = upperVanillaRarity;

            Color = color;
        }

        /// <summary>
        /// Rarity ID assigned from the <see cref="ModRarityLoader"/>.
        /// Will always be greater than the highest positive vanilla rarity unless the <see cref="ModRarityLoader"/> crashes,
        /// in which case it will take the value of <see cref="LowerVanillaRarity"/>.
        /// </summary>
        public int Id { get; internal set; }

        /// <summary>Used in comparing two different rarity IDs and fallback rarity ID in case the <see cref="ModRarityLoader"/> crashes.</summary>
        public int LowerVanillaRarity { get; }

        public int UpperVanillaRarity { get; }

        /// <summary>The displayed color. You can return something dynamic like <example>new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB)</example>.</summary>
        public virtual Color Color { get; }
    }
}
