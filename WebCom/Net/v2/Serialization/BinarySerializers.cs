using Microsoft.Xna.Framework;
using MonoMod.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using WebCom.Serializers;

namespace WebCom.Net.v2.Serialization;

/// <summary>Provides the methods for a specific datatype to be handled when received or sent.</summary>
public class BinarySerializer : Serializer<BinarySerializer.DataReader, BinarySerializer.DataWriter>
{
    public delegate object DataReader(BinaryReader reader);
    public delegate void DataWriter(BinaryWriter writer, object value);

    public BinarySerializer(DataReader reader, DataWriter writer)
    {
        Reader = reader;
        Writer = writer;
    }
}

public abstract class BinarySerializers : Serializers<BinarySerializer>
{
    internal class DefaultPacketSerializers : BinarySerializers
    {
        public DefaultPacketSerializers()
        {
            serializers.AddRange(new()
            {
                { typeof(bool),         new BinarySerializer(IOMethods.ReadBool,        IOMethods.WriteBool) },
                { typeof(char),         new BinarySerializer(IOMethods.ReadChar,        IOMethods.WriteChar) },
                { typeof(byte),         new BinarySerializer(IOMethods.ReadByte,        IOMethods.WriteByte) },
                { typeof(sbyte),        new BinarySerializer(IOMethods.ReadSByte,       IOMethods.WriteSByte) },
                { typeof(short),        new BinarySerializer(IOMethods.ReadShort,       IOMethods.WriteShort) },
                { typeof(ushort),       new BinarySerializer(IOMethods.ReadUShort,      IOMethods.WriteUShort) },
                { typeof(int),          new BinarySerializer(IOMethods.ReadInt,         IOMethods.WriteInt) },
                { typeof(uint),         new BinarySerializer(IOMethods.ReadUInt,        IOMethods.WriteUInt) },
                { typeof(long),         new BinarySerializer(IOMethods.ReadLong,        IOMethods.WriteLong) },
                { typeof(ulong),        new BinarySerializer(IOMethods.ReadULong,       IOMethods.WriteULong) },
                { typeof(float),        new BinarySerializer(IOMethods.ReadFloat,       IOMethods.WriteFloat) },
                { typeof(double),       new BinarySerializer(IOMethods.ReadDouble,      IOMethods.WriteDouble) },
                { typeof(decimal),      new BinarySerializer(IOMethods.ReadDecimal,     IOMethods.WriteDecimal) },
                { typeof(string),       new BinarySerializer(IOMethods.ReadString,      IOMethods.WriteString) },
                { typeof(Item),         new BinarySerializer(IOMethods.ReadItem,        IOMethods.WriteItem) },
                { typeof(Vector2),      new BinarySerializer(IOMethods.ReadVector2,     IOMethods.WriteVector2) },
                { typeof(Color),        new BinarySerializer(IOMethods.ReadRGB,         IOMethods.WriteRGB) },
                { typeof(BitsByte),     new BinarySerializer(IOMethods.ReadBitsByte,    IOMethods.WriteBitsByte) },
                { typeof(Rectangle),    new BinarySerializer(IOMethods.ReadRectangle,   IOMethods.WriteRectangle) },
                { typeof(Guid),         new BinarySerializer(IOMethods.ReadGuid,        IOMethods.WriteGuid) }
            });
        }
    }

    public class IOMethods
    {
        public static void WriteBool(BinaryWriter writer, object value) => writer.Write((bool)value);
        public static void WriteByte(BinaryWriter writer, object value) => writer.Write((byte)value);
        public static void WriteChar(BinaryWriter writer, object value) => writer.Write((char)value);
        public static void WriteDecimal(BinaryWriter writer, object value) => writer.Write((decimal)value);
        public static void WriteDouble(BinaryWriter writer, object value) => writer.Write((double)value);
        public static void WriteFloat(BinaryWriter writer, object value) => writer.Write((float)value);
        public static void WriteInt(BinaryWriter writer, object value) => writer.Write((int)value);
        public static void WriteLong(BinaryWriter writer, object value) => writer.Write((long)value);
        public static void WriteSByte(BinaryWriter writer, object value) => writer.Write((sbyte)value);
        public static void WriteShort(BinaryWriter writer, object value) => writer.Write((short)value);
        public static void WriteString(BinaryWriter writer, object value) => writer.Write((string)value);
        public static void WriteUInt(BinaryWriter writer, object value) => writer.Write((uint)value);
        public static void WriteULong(BinaryWriter writer, object value) => writer.Write((ulong)value);
        public static void WriteUShort(BinaryWriter writer, object value) => writer.Write((ushort)value);
        public static void WriteItem(BinaryWriter writer, object value) => ItemIO.Send((Item)value, writer, true, true);
        public static void WriteVector2(BinaryWriter writer, object value) => writer.WriteVector2((Vector2)value);
        public static void WriteRGB(BinaryWriter writer, object value) => writer.WriteRGB((Color)value);

        public static void WriteBitsByte(BinaryWriter writer, object value) => writer.Write((BitsByte)value);

        public static void WriteRectangle(BinaryWriter writer, object value)
        {
            Rectangle rectangle = (Rectangle)value;

            writer.Write(rectangle.X);
            writer.Write(rectangle.Y);
            writer.Write(rectangle.Width);
            writer.Write(rectangle.Height);
        }

        public static void WriteVector3(BinaryWriter writer, object value)
        {
            var vector = (Vector3)value;

            writer.Write(vector.X);
            writer.Write(vector.Y);
            writer.Write(vector.Z);
        }

        public static void WriteVector4(BinaryWriter writer, object value)
        {
            var vector = (Vector4)value;

            writer.Write(vector.X);
            writer.Write(vector.Y);
            writer.Write(vector.Z);
            writer.Write(vector.W);
        }

        public static void WriteNetworkSerializable(BinaryWriter writer, object value) => ((INetworkSerializable)value).Send(writer);

        public static void WriteGuid(BinaryWriter writer, object value) => writer.Write(((Guid)value).ToString());

        public static object ReadBool(BinaryReader reader) => reader.ReadBoolean();
        public static object ReadByte(BinaryReader reader) => reader.ReadByte();
        public static object ReadChar(BinaryReader reader) => reader.ReadChar();
        public static object ReadDecimal(BinaryReader reader) => reader.ReadDecimal();
        public static object ReadDouble(BinaryReader reader) => reader.ReadDouble();
        public static object ReadFloat(BinaryReader reader) => reader.ReadSingle();
        public static object ReadInt(BinaryReader reader) => reader.ReadInt32();
        public static object ReadLong(BinaryReader reader) => reader.ReadInt64();
        public static object ReadSByte(BinaryReader reader) => reader.ReadSByte();
        public static object ReadShort(BinaryReader reader) => reader.ReadInt16();
        public static object ReadString(BinaryReader reader) => reader.ReadString();
        public static object ReadUInt(BinaryReader reader) => reader.ReadUInt32();
        public static object ReadULong(BinaryReader reader) => reader.ReadUInt64();
        public static object ReadUShort(BinaryReader reader) => reader.ReadUInt16();
        public static object ReadItem(BinaryReader reader) => ItemIO.Receive(reader, true, true);
        public static object ReadVector2(BinaryReader reader) => reader.ReadVector2();
        public static object ReadRGB(BinaryReader reader) => reader.ReadRGB();

        public static object ReadBitsByte(BinaryReader reader) => (BitsByte)reader.ReadByte();

        public static object ReadRectangle(BinaryReader reader) => new Rectangle(
            reader.ReadInt32(), reader.ReadInt32(),
            reader.ReadInt32(), reader.ReadInt32());

        public static object ReadVector3(BinaryReader reader) => new Vector3(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32());

        public static object ReadVector4(BinaryReader reader) => new Vector4(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32());


        public static void ReadNetworkSerializable(ModPacket packet, INetworkSerializable networkSerializable, BinaryReader reader) => networkSerializable.Receive(reader);

        public static object ReadGuid(BinaryReader reader) => Guid.Parse(reader.ReadString());
    }
}
