﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using WebmilioCommons.Networking;
using WebmilioCommons.Networking.Packets;

namespace WebmilioCommons.Extensions
{
    public static class PlayerExtensions
    {
        public static bool IsLocalPlayer(this Player player) => player.whoAmI == Main.LocalPlayer.whoAmI;
        public static bool IsLocalPlayer(this ModPlayer modPlayer) => IsLocalPlayer(modPlayer.player);


        public static Player GetNearestMiningPlayer(this Vector2 position)
        {
            Player nearestPlayer = null;
            float nearestDistance = float.MaxValue;

            foreach (Player player in Main.player)
            {
                if (!player.active || player.itemAnimation == 0 || player.HeldItem == null || player.hitTile != null || !IsHoldingMiningItem(player)) continue;

                float distance = Vector2.Distance(position, player.position / 16);

                if (distance < nearestDistance)
                {
                    nearestPlayer = player;
                    nearestDistance = distance;
                }
            }

            return nearestPlayer;
        }

        public static bool IsHoldingMiningItem(this Player player)
        {
            Item item = player.HeldItem;

            if (item == null || !item.active)
                return false;

            return item.pick > 0 || item.axe > 0 || item.hammer > 0;
        }
        

        public static Tile GetTileOnCenter(this ModPlayer modPlayer) => GetTileOnCenter(modPlayer.player);
        public static Tile GetTileOnCenter(this ModNPC modNPC) => GetTileOnCenter(modNPC.npc);
        public static Tile GetTileOnCenter(this ModProjectile modProjectile) => GetTileOnCenter(modProjectile.projectile);

        public static Tile GetTileOnCenter(this Entity entity) => Main.tile[(int) (entity.Center.X / 16), (int) (entity.Center.Y / 16)];


        #region Packets

        public static void SendIfLocal(this Player player, NetworkPacket networkPacket, int? fromWho = null, int? toWho = null)
        {
            if (player.IsLocalPlayer())
                networkPacket.Send(fromWho, toWho);
        }

        public static void SendIfLocal(this ModPlayer modPlayer, NetworkPacket networkPacket, int? fromWho = null, int? toWho = null) =>
            SendIfLocal(modPlayer.player, networkPacket, fromWho, toWho);


        public static void SendIfLocal<T>(this Player player, int? fromWho = null, int? toWho = null) where T : NetworkPacket
        {
            if (player.IsLocalPlayer())
                NetworkPacketLoader.Instance.SendPacket<T>(fromWho, toWho);
        }

        public static void SendIfLocal<T>(this ModPlayer modPlayer, int? fromWho = null, int? toWho = null) where T : NetworkPacket =>
            SendIfLocal<T>(modPlayer.player, fromWho, toWho);

        #endregion
    }
}
