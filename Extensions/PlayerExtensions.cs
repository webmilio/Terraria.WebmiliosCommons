using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using WebmilioCommons.Networking;
using WebmilioCommons.Networking.Packets;

namespace WebmilioCommons.Extensions
{
    public static class PlayerExtensions
    {
        public static Player GetNearestMiningPlayer(this Vector2 position)
        {
            Player nearestPlayer = null;
            float nearestDistance = float.MaxValue;

            foreach (Player player in Main.player)
            {
                if (!player.active || player.itemAnimation == 0 || player.HeldItem == null || player.HeldItem.pick == 0 || player.hitTile != null) continue;

                float distance = Vector2.Distance(position, player.position);

                if (distance < nearestDistance)
                {
                    nearestPlayer = player;
                    nearestDistance = distance;
                }
            }

            return nearestPlayer;
        }


        #region Packets

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

        #endregion
    }
}
