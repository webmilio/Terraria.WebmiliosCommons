using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using WebmilioCommons.Networking;
using WebmilioCommons.Networking.Packets.TileEntities;

namespace WebmilioCommons.Extensions
{
    public static class TileEntityExtensions
    {
        public static void SendIfServer<TPacket>(this TileEntity tileEntity) where TPacket : TileEntityNetworkPacket
        {
            if (Main.netMode != NetmodeID.Server)
                return;

            NetworkPacketLoader.Instance.SendTileEntityPacket<TPacket>(tileEntity);
        }

        public static void SendIfServer<TTileEntity>(this TTileEntity tileEntity, ModTileEntityNetworkPacket<TTileEntity> packet) where TTileEntity : TileEntity
        {
            if (Main.netMode != NetmodeID.Server)
                return;

            NetworkPacketLoader.Instance.SendTileEntityPacket(packet, tileEntity);
        }
    }
}