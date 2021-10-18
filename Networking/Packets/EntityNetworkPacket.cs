using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using WebmilioCommons.Commons;

namespace WebmilioCommons.Networking.Packets
{
    public abstract class EntityNetworkPacket : NetworkPacket
    {
        protected EntityNetworkPacket()
        {
        }

        protected EntityNetworkPacket(Entity entity)
        {
            Entity = entity;
        }


        internal override bool DoPreReceive(BinaryReader reader, int fromWho)
        {
            EntityTypeId = reader.ReadByte();
            Entity = GetEntity(reader.ReadInt32());

            return Entity != default;
        }

        protected override void PrePopulatePacket(ModPacket modPacket, ref int fromWho, ref int toWho)
        {
            if (Entity == default)
                throw new ArgumentException("Entity was not set before populating the packet!");

            modPacket.Write(EntityTypeId);
            modPacket.Write(Entity.whoAmI);
        }


        private Entity GetEntity(int whoAmI)
        {
            switch (EntityType)
            {
                case EntityType.Item:
                    return Main.item[whoAmI];
                case EntityType.NPC:
                    return Main.npc[whoAmI];
                case EntityType.Player:
                    return Main.player[whoAmI];
                case EntityType.Projectile:
                    return Main.projectile[whoAmI];
            }

            return default;
        }


        public byte EntityTypeId
        {
            get => (byte) EntityType;
            set => EntityType = (EntityType) value;
        }

        [NotMapped]
        public EntityType EntityType { get; set; }

        [NotMapped]
        public Entity Entity
        {
            get => (Entity) ContextEntity;
            set
            {
                switch (value)
                {
                    case Item _:
                        EntityType = EntityType.Item;
                        break;
                    case NPC _:
                        EntityType = EntityType.NPC;
                        break;
                    case Player _:
                        EntityType = EntityType.Player;
                        break;
                    case Projectile _:
                        EntityType = EntityType.Projectile;
                        break;
                    default:
                        throw new NotImplementedException("There is no corresponding enum type to the entity class.");
                }

                ContextEntity = value;
            }
        }
    }
}