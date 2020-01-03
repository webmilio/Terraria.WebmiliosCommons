using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Terraria.DataStructures;
using Terraria.ModLoader;
using WebmilioCommons.Loaders;
using WebmilioCommons.Networking.Packets;
using WebmilioCommons.Networking.Packets.TileEntities;

namespace WebmilioCommons.Networking
{
    public class NetworkPacketLoader : SingletonLoader<NetworkPacketLoader, NetworkPacket>
    {
        public delegate void PacketReceivedDelegate(NetworkPacket packet, BinaryReader reader);
        public delegate void PacketSentDelegate(NetworkPacket packet);


        public NetworkPacketLoader() : base(typeInfo => typeInfo.GetCustomAttribute<ObsoleteAttribute>() == null)
        {
        }


        public override void PreLoad()
        {
            NetworkPacket.Initialize();
        }

        protected override void PostUnload()
        {
            NetworkPacket.Unload();
        }

        /// <summary>Main method to hook into: redirect to this in your Mod's HandlePacket.</summary>
        /// <param name="reader"></param>
        /// <param name="fromWho"></param>
        public void HandlePacket(BinaryReader reader, int fromWho)
        {
            int typeId = reader.ReadInt32();
            NetworkPacket packet = New(typeId);

            PacketReceived?.Invoke(packet, reader);

            packet.Receive(reader, fromWho);
        }


        public void SendTileEntityPacket<TPacket>(TileEntity tileEntity) where TPacket : TileEntityNetworkPacket => SendTileEntityPacket(New<TPacket>(), tileEntity);

        public void SendTileEntityPacket(TileEntityNetworkPacket packet, TileEntity tileEntity) => packet.Send(tileEntity);


        public void SendPacket(NetworkPacket packet, int? fromWho = null, int? toWho = null)
        {
            if (packet is TileEntityNetworkPacket)
                throw new Exception($"You should use the {nameof(SendTileEntityPacket)} method to send ${nameof(TileEntityNetworkPacket)}.");

            packet.Send(fromWho, toWho);
        }

        public void SendPacket(int id, int? fromWho = null, int? toWho = null) => SendPacket(New(id), fromWho, toWho);
        public void SendPacket(Type type, int? fromWho = null, int? toWho = null) => SendPacket(GetId(type), fromWho, toWho);
        public void SendPacket<TPacket>(int? fromWho = null, int? toWho = null) where TPacket : NetworkPacket => SendPacket(typeof(TPacket), fromWho, toWho);


        internal static void OnPacketSent(NetworkPacket packet) => PacketSent?.Invoke(packet);

        public static event PacketReceivedDelegate PacketReceived;
        public static event PacketSentDelegate PacketSent;
    }
}