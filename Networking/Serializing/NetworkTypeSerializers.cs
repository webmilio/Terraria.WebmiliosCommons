using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using MonoMod.Utils;
using Terraria;
using Terraria.ModLoader;
using WebmilioCommons.Networking.Packets;

namespace WebmilioCommons.Networking.Serializing
{
    /// <summary>Class which manages the different serialization/deserialization methods for the <see cref="NetworkPacketLoader"/> and <see cref="NetworkPacket"/>.</summary>
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
                // { typeof(Item), new NetworkTypeSerializer(NetworkPacketIOExtensions.ReadItem, NetworkPacketIOExtensions.WriteItem) },
                { typeof(Vector2), new NetworkTypeSerializer(NetworkPacketIOExtensions.ReadVector2, NetworkPacketIOExtensions.WriteVector2) },
                { typeof(Color), new NetworkTypeSerializer(NetworkPacketIOExtensions.ReadRGB, NetworkPacketIOExtensions.WriteRGB) },

                { typeof(BitsByte), new NetworkTypeSerializer(NetworkPacketIOExtensions.ReadBitsByte, NetworkPacketIOExtensions.WriteBitsByte) },
                { typeof(Rectangle), new NetworkTypeSerializer(NetworkPacketIOExtensions.ReadRectangle, NetworkPacketIOExtensions.WriteRectangle) }
            };
        }

        internal static void Unload()
        {
            _serializers?.Clear();
            _serializers = null;
        }

        /// <summary>Add a network property serializer for the given type. Must be called in your <see cref="Mod.Load"/> method.</summary>
        /// <typeparam name="T">The property type.</typeparam>
        /// <param name="serializer">The serializer (reader/writer) for the type.</param>
        public static void AddSerializer<T>(NetworkTypeSerializer serializer) => AddSerializer(typeof(T), serializer);

        /// <summary>Add a network property serializer for the given type. Must be called in your <see cref="Mod.Load"/> method.</summary>
        /// <param name="type">The property type.</param>
        /// <param name="serializer">The serializer (reader/writer) for the type.</param>
        public static void AddSerializer(Type type, NetworkTypeSerializer serializer) => _serializers.Add(type, serializer);

        /// <summary>Add a range of property serializers for their given types. Must be called in your <see cref="Mod.Load"/> method.</summary>
        public static void AddSerializers(Dictionary<Type, NetworkTypeSerializer> serializers)
        {
            foreach (KeyValuePair<Type, NetworkTypeSerializer> serializer in serializers)
                _serializers.Add(serializer.Key, serializer.Value);
        }

        /// <summary>Check if there is a serializer defined for a type.</summary>
        /// <typeparam name="T">The property type.</typeparam>
        /// <returns><c>true</c> if there is a serializer for the type; otherwise false.</returns>
        public static bool Has<T>() => Has(typeof(T));

        /// <summary>Check if there is a serializer defined for a type.</summary>
        /// <param name="type">The property type.</param>
        /// <returns><c>true</c> if there is a serializer for the type; otherwise false.</returns>
        public static bool Has(Type type) => _serializers.ContainsKey(type);

        /// <summary>Fetch a property serializer.</summary>
        /// <typeparam name="T">The property type.</typeparam>
        /// <returns>The <see cref="NetworkTypeSerializer"/> if found; otherwise <c>null</c>.</returns>
        public static NetworkTypeSerializer Get<T>() => Get(typeof(T));

        /// <summary>Fetch a property serializer.</summary>
        /// <param name="type">The property type.</param>
        /// <returns>The <see cref="NetworkTypeSerializer"/> if found; otherwise <c>null</c>.</returns>
        public static NetworkTypeSerializer Get(Type type) => _serializers[type];
    }
}