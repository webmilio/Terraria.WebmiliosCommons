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


        private static Dictionary<Type, NetworkTypeSerializer> _serializers;


        public NetworkPacketLoader() : base(typeInfo => typeInfo.GetCustomAttribute<ObsoleteAttribute>() == null)
        {
        }


        public override void PreLoad()
        {
            _serializers = new Dictionary<Type, NetworkTypeSerializer>
            {
                { typeof(bool), new NetworkTypeSerializer(NetworkPacketIOExtensions.ReadBool, NetworkPacketIOExtensions.WriteBool) },
                { typeof(byte), new NetworkTypeSerializer(NetworkPacketIOExtensions.ReadByte, NetworkPacketIOExtensions.WriteByte) },
                { typeof(char), new NetworkTypeSerializer(NetworkPacketIOExtensions.ReadChar, NetworkPacketIOExtensions.WriteChar) },
                { typeof(byte), new NetworkTypeSerializer(NetworkPacketIOExtensions.ReadByte, NetworkPacketIOExtensions.WriteByte) },
                { typeof(sbyte), new NetworkTypeSerializer(NetworkPacketIOExtensions.ReadSByte, NetworkPacketIOExtensions.WriteSByte) },
                { typeof(short), new NetworkTypeSerializer(NetworkPacketIOExtensions.ReadShort, NetworkPacketIOExtensions.WriteShort) },
                { typeof(ushort), new NetworkTypeSerializer(NetworkPacketIOExtensions.ReadUShort, NetworkPacketIOExtensions.WriteUShort) },
                { typeof(int), new NetworkTypeSerializer(NetworkPacketIOExtensions.ReadInt, NetworkPacketIOExtensions.WriteInt) },
                { typeof(uint), new NetworkTypeSerializer(NetworkPacketIOExtensions.ReadUInt, NetworkPacketIOExtensions.WriteUInt) },
                { typeof(long), new NetworkTypeSerializer(NetworkPacketIOExtensions.ReadLong, NetworkPacketIOExtensions.WriteLong) },
                { typeof(ulong), new NetworkTypeSerializer(NetworkPacketIOExtensions.ReadULong, NetworkPacketIOExtensions.WriteULong) },
                { typeof(float), new NetworkTypeSerializer(NetworkPacketIOExtensions.ReadFloat, NetworkPacketIOExtensions.WriteFloat) },
                { typeof(double), new NetworkTypeSerializer(NetworkPacketIOExtensions.ReadDouble, NetworkPacketIOExtensions.WriteDouble) },
                { typeof(decimal), new NetworkTypeSerializer(NetworkPacketIOExtensions.ReadDecimal, NetworkPacketIOExtensions.WriteDecimal) },
                { typeof(string), new NetworkTypeSerializer(NetworkPacketIOExtensions.ReadString, NetworkPacketIOExtensions.WriteString) },
                { typeof(Item), new NetworkTypeSerializer(NetworkPacketIOExtensions.ReadItem, NetworkPacketIOExtensions.WriteItem) },
                { typeof(Vector2), new NetworkTypeSerializer(NetworkPacketIOExtensions.ReadVector2, NetworkPacketIOExtensions.WriteVector2) },
                { typeof(Color), new NetworkTypeSerializer(NetworkPacketIOExtensions.ReadRGB, NetworkPacketIOExtensions.WriteRGB) }
            };


            NetworkPacket.Initialize();
        }

        public override void PostLoad()
        {
            int lastPacketIndex = NextIndex - 1;

            if (lastPacketIndex <= byte.MaxValue)
            {
                PacketIdWriter = (packet, modPacket, value) => packet.WriteByte(modPacket, (byte)(int)value);
                PacketIdReader = NetworkPacketIOExtensions.ReadByte;
            }
            else if (lastPacketIndex <= short.MaxValue)
            {
                PacketIdWriter = (packet, modPacket, value) => packet.WriteShort(modPacket, (short)(int)value);
                PacketIdReader = NetworkPacketIOExtensions.ReadShort;
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

            _serializers.Clear();
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


        public void AddSerializer<T>(NetworkTypeSerializer serializer) => AddSerializer(typeof(T), serializer);
        public void AddSerializer(Type type, NetworkTypeSerializer serializer) => _serializers.Add(type, serializer);

        public bool HasSerializer<T>() => HasSerializer(typeof(T));
        public bool HasSerializer(Type type) => _serializers.ContainsKey(type);

        public NetworkTypeSerializer GetSerializer<T>() => GetSerializer(typeof(T));
        public NetworkTypeSerializer GetSerializer(Type type) => _serializers[type];

        public Func<NetworkPacket, BinaryReader, object> PacketIdReader { get; private set; }

        public Action<NetworkPacket, ModPacket, object> PacketIdWriter { get; private set; }


        internal static void OnPacketSent(NetworkPacket packet) => PacketSent?.Invoke(packet);

        public static event PacketReceivedDelegate PacketReceived;
        public static event PacketSentDelegate PacketSent;
    }
}