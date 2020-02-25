using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using WebmilioCommons.Networking.Serializing;

namespace WebmilioCommons.Networking.Packets
{
    public static class NetworkPacketIOExtensions
    {
        public static void WriteBool(this NetworkPacket networkPacket, ModPacket modPacket, object value) => modPacket.Write((bool)value);
        public static void WriteByte(this NetworkPacket networkPacket, ModPacket modPacket, object value) => modPacket.Write((byte)value);
        public static void WriteChar(this NetworkPacket networkPacket, ModPacket modPacket, object value) => modPacket.Write((char)value);
        public static void WriteDecimal(this NetworkPacket networkPacket, ModPacket modPacket, object value) => modPacket.Write((decimal)value);
        public static void WriteDouble(this NetworkPacket networkPacket, ModPacket modPacket, object value) => modPacket.Write((double)value);
        public static void WriteFloat(this NetworkPacket networkPacket, ModPacket modPacket, object value) => modPacket.Write((float)value);
        public static void WriteInt(this NetworkPacket networkPacket, ModPacket modPacket, object value) => modPacket.Write((int)value);
        public static void WriteLong(this NetworkPacket networkPacket, ModPacket modPacket, object value) => modPacket.Write((long)value);
        public static void WriteSByte(this NetworkPacket networkPacket, ModPacket modPacket, object value) => modPacket.Write((sbyte)value);
        public static void WriteShort(this NetworkPacket networkPacket, ModPacket modPacket, object value) => modPacket.Write((short)value);
        public static void WriteString(this NetworkPacket networkPacket, ModPacket modPacket, object value) => modPacket.Write((string)value);
        public static void WriteUInt(this NetworkPacket networkPacket, ModPacket modPacket, object value) => modPacket.Write((uint)value);
        public static void WriteULong(this NetworkPacket networkPacket, ModPacket modPacket, object value) => modPacket.Write((ulong)value);
        public static void WriteUShort(this NetworkPacket networkPacket, ModPacket modPacket, object value) => modPacket.Write((ushort)value);
        public static void WriteItem(this NetworkPacket networkPacket, ModPacket modPacket, object value) => modPacket.WriteItem((Item) value, true, true);
        public static void WriteVector2(this NetworkPacket networkPacket, ModPacket modPacket, object value) => modPacket.WriteVector2((Vector2) value);
        public static void WriteRGB(this NetworkPacket networkPacket, ModPacket modPacket, object value) => modPacket.WriteRGB((Color) value);

        public static void WriteBitsByte(this NetworkPacket networkPacket, ModPacket modPacket, object value) => modPacket.Write((BitsByte) value);

        public static void WriteNetworkSerializable(this NetworkPacket networkPacket, ModPacket modPacket, object value) => ((Serializing.INetworkSerializable)value).Send(networkPacket, modPacket);



        public static object ReadBool(this NetworkPacket networkPacket, BinaryReader reader) => reader.ReadBoolean();
        public static object ReadByte(this NetworkPacket networkPacket, BinaryReader reader) => reader.ReadByte();
        public static object ReadChar(this NetworkPacket networkPacket, BinaryReader reader) => reader.ReadChar();
        public static object ReadDecimal(this NetworkPacket networkPacket, BinaryReader reader) => reader.ReadDecimal();
        public static object ReadDouble(this NetworkPacket networkPacket, BinaryReader reader) => reader.ReadDouble();
        public static object ReadFloat(this NetworkPacket networkPacket, BinaryReader reader) => reader.ReadSingle();
        public static object ReadInt(this NetworkPacket networkPacket, BinaryReader reader) => reader.ReadInt32();
        public static object ReadLong(this NetworkPacket networkPacket, BinaryReader reader) => reader.ReadInt64();
        public static object ReadSByte(this NetworkPacket networkPacket, BinaryReader reader) => reader.ReadSByte();
        public static object ReadShort(this NetworkPacket networkPacket, BinaryReader reader) => reader.ReadInt16();
        public static object ReadString(this NetworkPacket networkPacket, BinaryReader reader) => reader.ReadString();
        public static object ReadUInt(this NetworkPacket networkPacket, BinaryReader reader) => reader.ReadUInt32();
        public static object ReadULong(this NetworkPacket networkPacket, BinaryReader reader) => reader.ReadUInt64();
        public static object ReadUShort(this NetworkPacket networkPacket, BinaryReader reader) => reader.ReadUInt16();
        public static object ReadItem(this NetworkPacket networkPacket, BinaryReader reader) => ItemIO.Receive(reader, true, true);
        public static object ReadVector2(this NetworkPacket networkPacket, BinaryReader reader) => reader.ReadVector2();
        public static object ReadRGB(this NetworkPacket networkPacket, BinaryReader reader) => reader.ReadRGB();

        public static object ReadBitsByte(this NetworkPacket networkPacket, BinaryReader reader) => (BitsByte) reader.ReadByte();

        public static void ReadNetworkSerializable(this NetworkPacket networkPacket, Serializing.INetworkSerializable networkSerializable, BinaryReader reader) => networkSerializable.Receive(networkPacket, reader);
    }
}