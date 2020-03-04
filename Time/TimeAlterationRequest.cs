using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using WebmilioCommons.Networking.Packets;
using WebmilioCommons.Networking.Serializing;
using WebmilioCommons.Tinq;

namespace WebmilioCommons.Time
{
    public class TimeAlterationRequest : INetworkSerializable
    {
        /// <summary>Instantiates an empty standard <see cref="TimeAlterationRequest"/>.</summary>
        public TimeAlterationRequest() { }

        /// <summary>Instantiates a standard <see cref="TimeAlterationRequest"/>.</summary>
        /// <param name="player">The player who initiated the time alteration.</param>
        /// <param name="duration">How long the time alteration should last.</param>
        /// <param name="tickRate">The rate at which the world ticks for everything else than the source. Only tick rates of 0 or 1 are supposed as of yet.</param>
        public TimeAlterationRequest(Player player, int duration, int tickRate) : this(Sources.Player, player, duration, tickRate)
        {
        }

        /// <summary>Instantiates a standard <see cref="TimeAlterationRequest"/>.</summary>
        /// <param name="npc">The player who initiated the time alteration.</param>
        /// <param name="duration">How long the time alteration should last.</param>
        /// <param name="tickRate">The rate at which the world ticks for everything else than the source. Only tick rates of 0 or 1 are supposed as of yet.</param>
        public TimeAlterationRequest(NPC npc, int duration, int tickRate) : this(Sources.NPC, npc, duration, tickRate)
        {
        }

        /// <summary>Instantiates a standard <see cref="TimeAlterationRequest"/>.</summary>
        /// <param name="projectile">The npc who initiated the time alteration.</param>
        /// <param name="duration">How long the time alteration should last.</param>
        /// <param name="tickRate">The rate at which the world ticks for everything else than the source. Only tick rates of 0 or 1 are supposed as of yet.</param>
        public TimeAlterationRequest(Projectile projectile, int duration, int tickRate) : this(Sources.Projectile, projectile, duration, tickRate)
        {
        }

        /// <summary>Instantiates a standard <see cref="TimeAlterationRequest"/>.</summary>
        /// <param name="item">The item which initiated the time alteration.</param>
        /// <param name="duration">How long the time alteration should last.</param>
        /// <param name="tickRate">The rate at which the world ticks for everything else than the source. Only tick rates of 0 or 1 are supposed as of yet.</param>
        public TimeAlterationRequest(Item item, int duration, int tickRate) : this(Sources.Item, item, duration, tickRate)
        {
        }


        /// <summary>
        /// </summary>
        /// <param name="sourceType"></param>
        /// <param name="duration"></param>
        /// <param name="tickRate">The rate at which the world ticks for everything else than the source. Only tick rates of 0 or 1 are supposed as of yet.</param>
        public TimeAlterationRequest(Sources sourceType, int duration, int tickRate) : this(sourceType, null, duration, tickRate)
        {
            if (sourceType.HasFlag(Sources.Entity))
                throw new ArgumentException("Use a constructor that takes an Entity when creating a request from an entity.");
        }

        internal TimeAlterationRequest(Sources sourceType, Entity sourceEntity, int duration, int tickRate)
        {
            SourceType = sourceType;
            SourceEntity = sourceEntity;

            Duration = duration;
            TickRate = tickRate;
        }


        /// <summary>Writes the request's information to the provided <see cref="ModPacket"/>. To add information to the <see cref="ModPacket"/>, override <see cref="ModSend"/>.</summary>
        /// <param name="networkPacket"></param>
        /// <param name="modPacket"></param>
        public void Send(NetworkPacket networkPacket, ModPacket modPacket)
        {
            networkPacket.WriteString(modPacket, SourceType.ToString());
            networkPacket.WriteInt(modPacket, SourceEntity.whoAmI);

            networkPacket.WriteInt(modPacket, Duration);
            networkPacket.WriteInt(modPacket, TickRate);

            networkPacket.WriteBool(modPacket, LockedToSource);

            networkPacket.WriteBool(modPacket, AlterPlayers);
            networkPacket.WriteBool(modPacket, AlterItems);
            networkPacket.WriteBool(modPacket, AlterNPCs);
            networkPacket.WriteBool(modPacket, AlterProjectiles);

            networkPacket.WriteInt(modPacket, DayRate);
            networkPacket.WriteInt(modPacket, TimeRate);
            networkPacket.WriteInt(modPacket, RainRate);

            ModSend(networkPacket, modPacket);
        }

        /// <summary></summary>
        /// <param name="networkPacket"></param>
        /// <param name="modPacket"></param>
        public virtual void ModSend(NetworkPacket networkPacket, ModPacket modPacket) { }


        /// <summary>Reads the request's information to the provided <see cref="ModPacket"/>. To receive additional information to the <see cref="ModPacket"/>, override <see cref="ModReceive"/>.</summary>
        /// <param name="networkPacket"></param>
        /// <param name="reader"></param>
        public void Receive(NetworkPacket networkPacket, BinaryReader reader)
        {
            SourceType = (Sources) Enum.Parse(typeof(Sources), reader.ReadString());

            IEnumerable<Entity> sourceEntityCollection = null;

            switch (SourceType)
            {
                case Sources.Player:
                    sourceEntityCollection = Main.player;
                    break;
                case Sources.NPC:
                    sourceEntityCollection = Main.npc;
                    break;
                case Sources.Projectile:
                    sourceEntityCollection = Main.projectile;
                    break;
                case Sources.Item:
                    sourceEntityCollection = Main.item;
                    break;
            }

            if (sourceEntityCollection != null)
                SourceEntity = sourceEntityCollection.FirstActive(e => e.whoAmI == reader.ReadInt32());

            Duration = reader.ReadInt32();
            TickRate = reader.ReadInt32();

            LockedToSource = reader.ReadBoolean();

            AlterPlayers = reader.ReadBoolean();
            AlterItems = reader.ReadBoolean();
            AlterNPCs = reader.ReadBoolean();
            AlterProjectiles = reader.ReadBoolean();

            DayRate = reader.ReadInt32();
            TimeRate = reader.ReadInt32();
            RainRate = reader.ReadInt32();

            ModReceive(networkPacket, reader);
        }

        /// <summary></summary>
        /// <param name="networkPacket"></param>
        /// <param name="modPacket"></param>
        public virtual void ModReceive(NetworkPacket networkPacket, BinaryReader reader) { }


        public Sources SourceType { get; private set; }

        public bool StoppedByEntity => SourceEntity != null;
        public Entity SourceEntity { get; private set; }


        public int Duration { get; private set; }
        public int TickRate { get; private set; }

        /// <summary>
        /// If true, the time alteration will only be able to be "toggled" (stopped midway) if the same person executes the required action.
        /// Other entities/sources are unable to change the time state if this is true.
        /// </summary>
        public bool LockedToSource { get; set; } = true;


        public virtual bool AlterPlayers { get; set; } = true;
        public virtual bool AlterItems { get; set; } = true;
        // ReSharper disable once InconsistentNaming
        public virtual bool AlterNPCs { get; set; } = true;
        public virtual bool AlterProjectiles { get; set; } = true;

        public virtual int DayRate
        {
            get => TickRate; 
            set => TickRate = value;
        }

        public virtual int TimeRate
        {
            get => TickRate;
            set => TickRate = value;
        }

        public virtual int RainRate
        {
            get => TickRate;
            set => TickRate = value;
        }


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