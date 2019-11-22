using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Terraria.ModLoader;
using WebmilioCommons.Loaders;
using WebmilioCommons.Networking.Packets;

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
            NetworkPacket.GlobalReflectedPropertyInfos = new Dictionary<Type, List<PropertyInfo>>();

            NetworkPacket.GlobalPacketReaders = new Dictionary<Type, Dictionary<PropertyInfo, Func<NetworkPacket, BinaryReader, object>>>();
            NetworkPacket.GlobalPacketWriters = new Dictionary<Type, Dictionary<PropertyInfo, Action<ModPacket, object>>>();
        }

        public override void Unload()
        {
            NetworkPacket.GlobalReflectedPropertyInfos = null;

            NetworkPacket.GlobalPacketReaders = null;
            NetworkPacket.GlobalPacketWriters = null;

            base.Unload();
        }

        /// <summary>Main method to hook into: redirect to this in your Mod's HandlePacket.</summary>
        /// <param name="reader"></param>
        /// <param name="fromWho"></param>
        public void HandlePacket(BinaryReader reader, int fromWho)
        {
            ushort typeId = reader.ReadUInt16();
            NetworkPacket packet = New(typeId);

            PacketReceived?.Invoke(packet, reader);

            packet.Receive(reader, fromWho);
        }


        public void SendPacket(ushort id, int? fromWho = null, int? toWho = null) => New(id).Send(fromWho, toWho);
        public void SendPacket(Type type, int? fromWho = null, int? toWho = null) => New(type).Send(fromWho, toWho);
        public void SendPacket<T>(int? fromWho = null, int? toWho = null) where T : NetworkPacket => New<T>().Send(fromWho, toWho);


        internal static void OnPacketSent(NetworkPacket packet) => PacketSent?.Invoke(packet);

        public static event PacketReceivedDelegate PacketReceived;
        public static event PacketSentDelegate PacketSent;
    }
}