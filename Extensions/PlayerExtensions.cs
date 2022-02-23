using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using WebmilioCommons.Networking;
using WebmilioCommons.Networking.Packets;
using WebmilioCommons.Tinq;

namespace WebmilioCommons.Extensions
{
    public static class PlayerExtensions
    {
        public static ProjectileSource_Item AsItemProjectileSource(this Player player) => new(player, player.HeldItem);


        public static bool IsLocalPlayer(this Player player) => player.whoAmI == Main.myPlayer;
        public static bool IsLocalPlayer(this ModPlayer modPlayer) => IsLocalPlayer(modPlayer.Player);


        public static bool IsServerOwner(this Player player) => Main.netMode == NetmodeID.SinglePlayer || Netplay.Connection.Socket.GetRemoteAddress().IsLocalHost();
        public static bool IsServerOwner(this ModPlayer modPlayer) => IsServerOwner(modPlayer.Player);


        public static void DoIfLocal(this Player player, Action<Player> action)
        {
            if (IsLocalPlayer(player))
                action(player);
        }

        public static void DoIfLocal<T>(this T modPlayer, Action<T> action) where T : ModPlayer
        {
            if (IsLocalPlayer(modPlayer))
                action(modPlayer);
        }


        public static Player GetNearestMiningPlayer(this Vector2 position)
        {
            Player nearestPlayer = default;
            float nearestDistance = float.MaxValue;

            foreach (Player player in Main.player.Active())
            {
                if (player.itemAnimation == 0 || player.HeldItem == null || player.hitTile == null || !IsHoldingMiningItem(player)) 
                    continue;

                float distance = Vector2.Distance(position, player.position / 16);

                if (distance < nearestDistance)
                {
                    nearestPlayer = player;
                    nearestDistance = distance;
                }
            }

            return nearestPlayer;
        }

        /// <summary>Checks if the player is holding an item capable of breaking blocks (pickaxes, axes, hammers).</summary>
        /// <param name="player"></param>
        /// <returns><c>true</c> if the player is holding a mining item; <c>false</c> otherwise.</returns>
        public static bool IsHoldingMiningItem(this Player player)
        {
            Item item = player.HeldItem;

            if (item == default || !item.active)
                return false;

            return item.pick > 0 || item.axe > 0 || item.hammer > 0;
        }

        public static bool IsHoldingMiningItem(this ModPlayer modPlayer) => IsHoldingMiningItem(modPlayer.Player);


        public static Tile GetTileOnCenter(this ModPlayer modPlayer) => GetTileOnCenter(modPlayer.Player);
        public static Tile GetTileOnCenter(this ModNPC modNPC) => GetTileOnCenter(modNPC.NPC);
        public static Tile GetTileOnCenter(this ModProjectile modProjectile) => GetTileOnCenter(modProjectile.Projectile);

        public static Tile GetTileOnCenter(this Entity entity) => Main.tile[(int) (entity.Center.X / 16), (int) (entity.Center.Y / 16)];


        public static void QuickSpawnItem(this Player player, ModItem item) => player.QuickSpawnItem(new Item().SetDefaults(item));


        #region Packets

        public static void SendIfLocal(this Player player, NetworkPacket networkPacket, int? fromWho = null, int? toWho = null) => DoIfLocal(player, plr => networkPacket.Send(fromWho, toWho));

        public static void SendIfLocal(this ModPlayer modPlayer, NetworkPacket networkPacket, int? fromWho = null, int? toWho = null) => SendIfLocal(modPlayer.Player, networkPacket, fromWho, toWho);


        public static void SendIfLocal<T>(this Player player, int? fromWho = null, int? toWho = null) where T : NetworkPacket => DoIfLocal(player, plr => NetworkPacketLoader.Instance.SendPacket<T>(fromWho, toWho));

        public static void SendIfLocal<T>(this ModPlayer modPlayer, int? fromWho = null, int? toWho = null) where T : NetworkPacket => SendIfLocal<T>(modPlayer.Player, fromWho, toWho);

        #endregion
    }
}
