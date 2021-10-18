using System;
using Terraria;
using WebmilioCommons.Extensions;

namespace WebmilioCommons.Worlds.Generation.Structures
{
    public abstract class WorldStructure
    {
        private const string COORDINATE_OOB_EXCEPTION = "The {0} coordinates must be a positive integer under {1}.";


        /// <summary>Generates the structure from the top-left to the bottom-right starting at the given coordinates.</summary>
        /// <param name="x">The X coordinate of the origin tile..</param>
        /// <param name="y">The Y coordinate of the origin tile.</param>
        public void Generate(int x, int y)
        {
            if (x < 0)
                throw new ArgumentOutOfRangeException(string.Format(COORDINATE_OOB_EXCEPTION, "X", Main.tile.GetLength(0)));

            if (y < 0)
                throw new ArgumentOutOfRangeException(string.Format(COORDINATE_OOB_EXCEPTION, "Y", Main.tile.GetLength(1)));


        }


        public virtual string FrontLayerPath => $"{GetType().GetPath()}_front.png";
        public virtual string BackLayerPath => $"{GetType().GetPath()}_back.png";
        public virtual string WireLayerPath => $"{GetType().GetPath()}_wire.png";
        public virtual string LiquidLayerPath => $"{GetType().GetPath()}_liquid.png";
    }
}