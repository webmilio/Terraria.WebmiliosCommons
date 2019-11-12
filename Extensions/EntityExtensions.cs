using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using WebmilioCommons.Tinq;

namespace WebmilioCommons.Extensions
{
    public static class EntityExtensions
    {
        public const int
            FULL_CIRCLE_DEGREES = 360,
            HALF_CIRCLE_DEGREES = FULL_CIRCLE_DEGREES / 2;

        public static readonly float
            fullCircleRadians = MathHelper.ToRadians(FULL_CIRCLE_DEGREES),
            halfCircleRadians = fullCircleRadians / 2;


        public static T NearestActive<T>(this Entity entity, IEnumerable<T> entities) where T : Entity
        {
            T target = null;
            float distance = int.MaxValue;

            entities.DoActive(t =>
            {
                float newDistance = Vector2.Distance(entity.Center, t.Center);

                if (newDistance < distance)
                {
                    target = t;
                    distance = newDistance;
                }
            });

            return target;
        }

        public static float VelocityRotation(this Entity entity, bool degrees = false)
        {
            float rotation = entity.velocity.ToRotation();

            if (rotation < 0)
                rotation = fullCircleRadians + rotation;

            if (degrees)
                return MathHelper.ToDegrees(rotation);

            return rotation;
        }
    }
}