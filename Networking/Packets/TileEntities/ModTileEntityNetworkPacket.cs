using System;
using System.IO;
using Terraria.DataStructures;
using WebmilioCommons.Networking.Attributes;

namespace WebmilioCommons.Networking.Packets.TileEntities
{
    public abstract class ModTileEntityNetworkPacket<TTileEntity> : TileEntityNetworkPacket where TTileEntity : TileEntity
    {
        protected ModTileEntityNetworkPacket() : this(true) { }

        protected ModTileEntityNetworkPacket(bool autoGetProperties) : base(autoGetProperties)
        {
        }


        public override void Send(TileEntity tileEntity)
        {
            if (!(tileEntity is TTileEntity t))
                throw new ArgumentException($"An instance of {tileEntity.GetType().Name} was specified instead of a {typeof(TTileEntity).Namespace}.");

            ModTileEntity = t;
            base.Send(tileEntity);
        }

        protected override bool PreReceive(BinaryReader reader, int fromWho)
        {
            bool result = base.PreReceive(reader, fromWho);

            if (!result)
                return false;

            try
            {
                ModTileEntity = TileEntity as TTileEntity;
            }
            catch (InvalidCastException e)
            {
                throw new NetworkSynchronizationException($"TileEntity under ID {SupposedTileEntityID} is not a {nameof(TileEntity)} of type {typeof(TTileEntity).Name}.\n" +
                    $"Synchronization failed for packet {GetType().Name}.", e);
            }

            return ModTileEntity != null;
        }


        [NotNetworkField]
        public TTileEntity ModTileEntity { get; set; }
    }
}
