using Terraria;
using Terraria.ModLoader;
using WebmilioCommons.Networking;
using WebmilioCommons.Networking.Packets;

namespace WebmilioCommons.Extensions
{
    public static class PlayerExtensions
    {
        public static void SendIfLocal(this Player player, NetworkPacket networkPacket, int? fromWho = null, int? toWho = null)
        {
            if (player == Main.LocalPlayer)
                networkPacket.Send(fromWho, toWho);
        }

        public static void SendIfLocal(this ModPlayer modPlayer, NetworkPacket networkPacket, int? fromWho = null, int? toWho = null) =>
            SendIfLocal(modPlayer.player, networkPacket, fromWho, toWho);


        public static void SendIfLocal<T>(this Player player, int? fromWho = null, int? toWho = null) where T : NetworkPacket
        {
            if (player == Main.LocalPlayer)
                NetworkPacketLoader.Instance.SendPacket<T>(fromWho, toWho);
        }

        public static void SendIfLocal<T>(this ModPlayer modPlayer, int? fromWho = null, int? toWho = null) where T : NetworkPacket =>
            SendIfLocal<T>(modPlayer.player, fromWho, toWho);
    }
}
