using System;
using WebmilioCommons.Commons;

namespace WebmilioCommons.Extensions
{
    public static class DirectionsExtension
    {
        public static Directions GetOpposite(this Directions direction)
        {
            switch (direction)
            {
                case Directions.Up:
                    return Directions.Down;
                case Directions.Down:
                    return Directions.Up;
                case Directions.Left:
                    return Directions.Right;
                case Directions.Right:
                    return Directions.Left;
            }

            throw new ArgumentException("Unsupported direction, this shouldn't be happening.");
        }
    }
}