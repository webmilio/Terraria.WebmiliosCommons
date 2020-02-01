using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using WebmilioCommons.Networking.Packets;

namespace WebmilioCommons.Networking.Serializing
{
    public static class NetworkTypeSerializers
    {
        private static Dictionary<Type, NetworkTypeSerializer> _serializers;


        internal static void Initialize()
        {
            _serializers = new Dictionary<Type, NetworkTypeSerializer>
            {
                { typeof(bool), new NetworkTypeSerializer(NetworkPacketIOExtensions.ReadBool, NetworkPacketIOExtensions.WriteBool) },
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
        }

        internal static void Unload()
        {
            _serializers.Clear();
        }


        public static void AddSerializer<T>(NetworkTypeSerializer serializer) => AddSerializer(typeof(T), serializer);
        public static void AddSerializer(Type type, NetworkTypeSerializer serializer) => _serializers.Add(type, serializer);

        public static bool Has<T>() => Has(typeof(T));
        public static bool Has(Type type) => _serializers.ContainsKey(type);

        public static NetworkTypeSerializer Get<T>() => Get(typeof(T));
        public static NetworkTypeSerializer Get(Type type) => _serializers[type];
    }
}