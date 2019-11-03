using System;
using Terraria;

namespace WebmilioCommons.Time
{
    public class TimeAlterationRequest
    {
        public TimeAlterationRequest(Player player, int duration, int tickRate) : this(Sources.Player, player, duration, tickRate)
        {
        }

        public TimeAlterationRequest(NPC npc, int duration, int tickRate) : this(Sources.NPC, npc, duration, tickRate)
        {
        }

        public TimeAlterationRequest(Projectile projectile, int duration, int tickRate) : this(Sources.Projectile, projectile, duration, tickRate)
        {
        }

        public TimeAlterationRequest(Item item, int duration, int tickRate) : this(Sources.Item, item, duration, tickRate)
        {
        }


        /// <summary>
        /// </summary>
        /// <param name="sourceType"></param>
        /// <param name="duration"></param>
        /// <param name="tickRate">The rate at which the world ticks for everything else than the source. Only tick rates of 0 or 1 are supposed.</param>
        public TimeAlterationRequest(Sources sourceType, int duration, int tickRate) : this(sourceType, null, duration, tickRate)
        {
            if (sourceType.HasFlag(Sources.Entity))
                throw new ArgumentException("Use a constructor that takes an Entity when creating a request from an entity.");
        }

        private TimeAlterationRequest(Sources sourceType, Entity sourceEntity, int duration, int tickRate)
        {
            SourceType = sourceType;
            SourceEntity = sourceEntity;

            Duration = duration;
            TickRate = tickRate;
        }


        public Sources SourceType { get; }

        public bool StoppedByEntity => SourceEntity != null;
        public Entity SourceEntity { get; }


        public int Duration { get; }
        public int TickRate { get; }

        /// <summary>
        /// If true, the time alteration will only be able to be "toggled" (stopped midway) if the same person executes the required action.
        /// Other entities/sources are unable to change the time state if this is true.
        /// </summary>
        public bool LockedToSource { get; set; } = true;


        public bool AlterPlayers { get; set; } = true;
        public bool AlterNPCs { get; set; } = true;
        public bool AlterProjectiles { get; set; } = true;


        [Flags]
        public enum Sources
        {
            None = 0,

            Entity = 1 << 0,
            Player = Entity | 1 << 1,
            NPC = Entity | 1 << 2,
            Projectile = Entity | 1 << 3,
            Item = Entity | 1 << 4,

            World = 1 << 5,
            Tile = 1 << 6,
            SpecialEvent = 1 << 7
        }
    }
}