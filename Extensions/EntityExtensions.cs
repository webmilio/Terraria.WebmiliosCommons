using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using WebmilioCommons.Effects.Shaders;
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


        public static Vector2 ScreenPosition(this Entity entity) => entity.Center - Main.screenPosition;


        public static void Shade<T>(this Entity entity) where T : ShaderEffect => Shade<T>(entity, Main.spriteBatch);

        public static void Shade<T>(this Entity entity, SpriteBatch spriteBatch) where T : ShaderEffect
        {
            var shaderEffect = ShaderEffectsLoader.Instance.GetGeneric<T>();

            shaderEffect.Apply(spriteBatch, entity.ScreenPosition());
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

        /// <summary></summary>
        /// <param name="entity"></param>
        /// <param name="requireTileUnder"></param>
        /// <param name="feetXRadius"></param>
        /// <returns></returns>
        public static bool IsImmobile(this Entity entity, bool requireTileUnder = true, int feetXRadius = 0)
        {
            Vector2 velocity = entity.velocity;

            if (velocity.X != 0 && velocity.Y != 0)
                return false;

            Vector2 center = entity.Center;
            Point tileUnder = new Point((int) center.X, (int) (center.Y - entity.height / 2));

            if (feetXRadius == 0)
            {
                if (Main.tile[tileUnder.X, tileUnder.Y].type == 0)
                    return false;
            }


            return true;
        }
    }
}