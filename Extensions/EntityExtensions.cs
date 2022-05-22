using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using WebmilioCommons.Commons;

namespace WebmilioCommons.Extensions
{
    public static class EntityExtensions
    {
        public const int
            FullCircleDegrees = 360,
            HalfCircleDegrees = FullCircleDegrees / 2;

        public static readonly float
            fullCircleRadians = MathHelper.ToRadians(FullCircleDegrees),
            halfCircleRadians = fullCircleRadians / 2;


        public static EntityType GetEntityType(this Entity entity)
        {
            switch (entity)
            {
                case NPC:
                    return EntityType.NPC;
                case Player:
                    return EntityType.Player;
                case Projectile:
                    return EntityType.Projectile;
                case Item:
                    return EntityType.Item;
            }

            throw new InvalidOperationException("No corresponding Entity type found.");
        }

        public static Entity[] GetMainEntities(this EntityType entityType)
        {
            switch (entityType)
            {
                case EntityType.NPC:
                    return Main.npc;
                case EntityType.Player:
                    return Main.player;
                case EntityType.Projectile:
                    return Main.projectile;
                case EntityType.Item:
                    return Main.item;
            }

            return null;
        }

        public static Vector2 ScreenPosition(this Entity entity) => entity.Center - Main.screenPosition;

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
                if (Main.tile[tileUnder.X, tileUnder.Y].TileType == 0)
                    return false;
            }


            return true;
        }
    }
}