using Microsoft.Xna.Framework;
using MonoMod.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using WebCom.Serializers;

namespace WebCom.Networking.Serialization;

/// <summary>Provides the methods for a specific datatype to be handled when received or sent.</summary>
public class PacketSerializer : Serializer<PacketSerializer.DataReader, PacketSerializer.DataWriter>
{
    public delegate object DataReader(Packet packet, BinaryReader reader);
    public delegate void DataWriter(Packet packet, object value);

    public PacketSerializer(DataReader reader, DataWriter writer)
    {
        Reader = reader;
        Writer = writer;
    }
}

public abstract class PacketSerializers : Serializers<PacketSerializer>
{
    internal class DefaultPacketSerializers : PacketSerializers
    {
        public DefaultPacketSerializers()
        {
            serializers.AddRange(new()
            {
                { typeof(bool),         new PacketSerializer(IOMethods.ReadBool, IOMethods.WriteBool) },
                { typeof(char),         new PacketSerializer(IOMethods.ReadChar, IOMethods.WriteChar) },
                { typeof(byte),         new PacketSerializer(IOMethods.ReadByte, IOMethods.WriteByte) },
                { typeof(sbyte),        new PacketSerializer(IOMethods.ReadSByte, IOMethods.WriteSByte) },
                { typeof(short),        new PacketSerializer(IOMethods.ReadShort, IOMethods.WriteShort) },
                { typeof(ushort),       new PacketSerializer(IOMethods.ReadUShort, IOMethods.WriteUShort) },
                { typeof(int),          new PacketSerializer(IOMethods.ReadInt, IOMethods.WriteInt) },
                { typeof(uint),         new PacketSerializer(IOMethods.ReadUInt, IOMethods.WriteUInt) },
                { typeof(long),         new PacketSerializer(IOMethods.ReadLong, IOMethods.WriteLong) },
                { typeof(ulong),        new PacketSerializer(IOMethods.ReadULong, IOMethods.WriteULong) },
                { typeof(float),        new PacketSerializer(IOMethods.ReadFloat, IOMethods.WriteFloat) },
                { typeof(double),       new PacketSerializer(IOMethods.ReadDouble, IOMethods.WriteDouble) },
                { typeof(decimal),      new PacketSerializer(IOMethods.ReadDecimal, IOMethods.WriteDecimal) },
                { typeof(string),       new PacketSerializer(IOMethods.ReadString, IOMethods.WriteString) },
                { typeof(Item),         new PacketSerializer(IOMethods.ReadItem, IOMethods.WriteItem) },
                { typeof(Vector2),      new PacketSerializer(IOMethods.ReadVector2, IOMethods.WriteVector2) },
                { typeof(Color),        new PacketSerializer(IOMethods.ReadRGB, IOMethods.WriteRGB) },
                { typeof(BitsByte),     new PacketSerializer(IOMethods.ReadBitsByte, IOMethods.WriteBitsByte) },
                { typeof(Rectangle),    new PacketSerializer(IOMethods.ReadRectangle, IOMethods.WriteRectangle) }
            });
        }
    }

    public class IOMethods
    {
        public static void WriteBool(Packet packet, object value) => packet.ModPacket.Write((bool)value);
        public static void WriteByte(Packet packet, object value) => packet.ModPacket.Write((byte)value);
        public static void WriteChar(Packet packet, object value) => packet.ModPacket.Write((char)value);
        public static void WriteDecimal(Packet packet, object value) => packet.ModPacket.Write((decimal)value);
        public static void WriteDouble(Packet packet, object value) => packet.ModPacket.Write((double)value);
        public static void WriteFloat(Packet packet, object value) => packet.ModPacket.Write((float)value);
        public static void WriteInt(Packet packet, object value) => packet.ModPacket.Write((int)value);
        public static void WriteLong(Packet packet, object value) => packet.ModPacket.Write((long)value);
        public static void WriteSByte(Packet packet, object value) => packet.ModPacket.Write((sbyte)value);
        public static void WriteShort(Packet packet, object value) => packet.ModPacket.Write((short)value);
        public static void WriteString(Packet packet, object value) => packet.ModPacket.Write((string)value);
        public static void WriteUInt(Packet packet, object value) => packet.ModPacket.Write((uint)value);
        public static void WriteULong(Packet packet, object value) => packet.ModPacket.Write((ulong)value);
        public static void WriteUShort(Packet packet, object value) => packet.ModPacket.Write((ushort)value);
        public static void WriteItem(Packet packet, object value) => ItemIO.Send((Item)value, packet.ModPacket, true, true);
        public static void WriteVector2(Packet packet, object value) => packet.ModPacket.WriteVector2((Vector2)value);
        public static void WriteRGB(Packet packet, object value) => packet.ModPacket.WriteRGB((Color)value);

        public static void WriteBitsByte(Packet packet, object value) => packet.ModPacket.Write((BitsByte)value);

        public static void WriteRectangle(Packet packet, object value)
        {
            Rectangle rectangle = (Rectangle)value;

            packet.ModPacket.Write(rectangle.X);
            packet.ModPacket.Write(rectangle.Y);
            packet.ModPacket.Write(rectangle.Width);
            packet.ModPacket.Write(rectangle.Height);
        }

        public static void WriteVector3(Packet packet, object value)
        {
            Vector3 vector = (Vector3)value;

            packet.ModPacket.Write(vector.X);
            packet.ModPacket.Write(vector.Y);
            packet.ModPacket.Write(vector.Z);
        }

        public static void WriteVector4(Packet packet, object value)
        {
            Vector4 vector = (Vector4)value;

            packet.ModPacket.Write(vector.X);
            packet.ModPacket.Write(vector.Y);
            packet.ModPacket.Write(vector.Z);
            packet.ModPacket.Write(vector.W);
        }

        public static void WriteNetworkSerializable(Packet packet, object value) => ((INetworkSerializable)value).Send(packet);

        public static object ReadBool(Packet packet, BinaryReader reader) => reader.ReadBoolean();
        public static object ReadByte(Packet packet, BinaryReader reader) => reader.ReadByte();
        public static object ReadChar(Packet packet, BinaryReader reader) => reader.ReadChar();
        public static object ReadDecimal(Packet packet, BinaryReader reader) => reader.ReadDecimal();
        public static object ReadDouble(Packet packet, BinaryReader reader) => reader.ReadDouble();
        public static object ReadFloat(Packet packet, BinaryReader reader) => reader.ReadSingle();
        public static object ReadInt(Packet packet, BinaryReader reader) => reader.ReadInt32();
        public static object ReadLong(Packet packet, BinaryReader reader) => reader.ReadInt64();
        public static object ReadSByte(Packet packet, BinaryReader reader) => reader.ReadSByte();
        public static object ReadShort(Packet packet, BinaryReader reader) => reader.ReadInt16();
        public static object ReadString(Packet packet, BinaryReader reader) => reader.ReadString();
        public static object ReadUInt(Packet packet, BinaryReader reader) => reader.ReadUInt32();
        public static object ReadULong(Packet packet, BinaryReader reader) => reader.ReadUInt64();
        public static object ReadUShort(Packet packet, BinaryReader reader) => reader.ReadUInt16();
        public static object ReadItem(Packet packet, BinaryReader reader) => ItemIO.Receive(reader, true, true);
        public static object ReadVector2(Packet packet, BinaryReader reader) => reader.ReadVector2();
        public static object ReadRGB(Packet packet, BinaryReader reader) => reader.ReadRGB();

        public static object ReadBitsByte(Packet packet, BinaryReader reader) => (BitsByte)reader.ReadByte();

        public static object ReadRectangle(Packet packet, BinaryReader reader) => new Rectangle(
            reader.ReadInt32(), reader.ReadInt32(),
            reader.ReadInt32(), reader.ReadInt32());

        public static object ReadVector3(Packet packet, BinaryReader reader) => new Vector3(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32());

        public static object ReadVector4(Packet packet, BinaryReader reader) => new Vector4(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32());


        public static void ReadNetworkSerializable(Packet packet, INetworkSerializable networkSerializable, BinaryReader reader) => networkSerializable.Receive(packet, reader);
    }
}
