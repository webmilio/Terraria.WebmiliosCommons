using System;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using WebmilioCommons.Networking.Attributes;

namespace WebmilioCommons.Networking.Packets.TileEntities
{
    public abstract class TileEntityNetworkPacket : NetworkPacket
    {
        protected TileEntityNetworkPacket() : this(true) { }

        protected TileEntityNetworkPacket(bool autoGetProperties) : base(autoGetProperties)
        {
        }


        public virtual void Send(TileEntity tileEntity)
        {
            SupposedTileEntityID = tileEntity.ID;
            TileEntity = tileEntity;
            Send(Main.myPlayer, -1);
        }

        public sealed override void Send(int? fromWho = null, int? toWho = null) => base.Send(fromWho, toWho);

        protected override bool PreReceive(BinaryReader reader, int fromWho) => (TileEntity = TileEntity.ByID[SupposedTileEntityID = reader.ReadInt32()]) != default;


        [NotNetworkField]
        public int SupposedTileEntityID { get; private set; }

        [NotNetworkField]
        public TileEntity TileEntity
        {
            get => ContextEntity as TileEntity;
            set => ContextEntity = value;
        }
    }
}
