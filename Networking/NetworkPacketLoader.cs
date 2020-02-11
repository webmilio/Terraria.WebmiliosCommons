using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using WebmilioCommons.Loaders;
using WebmilioCommons.Networking.Packets;
using WebmilioCommons.Networking.Packets.TileEntities;
using WebmilioCommons.Networking.Serializing;

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
            NetworkTypeSerializers.Initialize();
            NetworkPacket.Initialize();
        }

        public override void PostLoad()
        {
            int lastPacketIndex = NextIndex - 1;

            if (lastPacketIndex <= byte.MaxValue)
            {
                PacketIdWriter = (packet, modPacket, value) => packet.WriteByte(modPacket, (byte)(int)value);
                PacketIdReader = (packet, reader) => (int)(byte)packet.ReadByte(reader);
            }
            else if (lastPacketIndex <= short.MaxValue)
            {
                PacketIdWriter = (packet, modPacket, value) => packet.WriteShort(modPacket, (short)(int)value);
                PacketIdReader = (packet, reader) => (int)(short)packet.ReadShort(reader);
            }
            else
            {
                PacketIdWriter = (packet, modPacket, value) => packet.WriteInt(modPacket, value);
                PacketIdReader = NetworkPacketIOExtensions.ReadInt;
            }
        }

        protected override void PostUnload()
        {
            NetworkPacket.Unload();
            NetworkTypeSerializers.Unload();
        }

        /// <summary>Main method to hook into: redirect to this in your Mod's HandlePacket.</summary>
        /// <param name="reader"></param>
        /// <param name="fromWho"></param>
        public void HandlePacket(BinaryReader reader, int fromWho)
        {
            int typeId = (int)PacketIdReader(null, reader);
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


        public Func<NetworkPacket, BinaryReader, object> PacketIdReader { get; private set; }

        public Action<NetworkPacket, ModPacket, object> PacketIdWriter { get; private set; }


        internal static void OnPacketSent(NetworkPacket packet) => PacketSent?.Invoke(packet);

        public static event PacketReceivedDelegate PacketReceived;
        public static event PacketSentDelegate PacketSent;
    }
}