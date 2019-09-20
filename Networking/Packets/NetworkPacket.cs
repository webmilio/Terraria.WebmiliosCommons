using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using WebmilioCommons.Networking.Attributes;

namespace WebmilioCommons.Networking.Packets
{
    public abstract class NetworkPacket
    {
        protected readonly List<PropertyInfo> reflectedPropertyInfos;
        protected readonly Dictionary<PropertyInfo, Action<ModPacket, object>> packetWriters = new Dictionary<PropertyInfo, Action<ModPacket, object>>();
        protected readonly Dictionary<PropertyInfo, Func<BinaryReader, object>> packetReaders = new Dictionary<PropertyInfo, Func<BinaryReader, object>>();


        protected NetworkPacket() : this(true) { }

        protected NetworkPacket(bool autoGetProperties)
        {
            reflectedPropertyInfos = new List<PropertyInfo>();

            if (autoGetProperties)
                AddAllProperties();
        }


        protected void AddReaderWriter(PropertyInfo propertyInfo)
        {
            if (propertyInfo.PropertyType == typeof(bool))
            {
                packetReaders.Add(propertyInfo, ReadBool);
                packetWriters.Add(propertyInfo, WriteBool);
            }
            else if (propertyInfo.PropertyType == typeof(byte))
            {
                packetReaders.Add(propertyInfo, ReadByte);
                packetWriters.Add(propertyInfo, WriteByte);
            }
            else if (propertyInfo.PropertyType == typeof(char))
            {
                packetReaders.Add(propertyInfo, ReadChar);
                packetWriters.Add(propertyInfo, WriteChar);
            }
            else if (propertyInfo.PropertyType == typeof(decimal))
            {
                packetReaders.Add(propertyInfo, ReadDecimal);
                packetWriters.Add(propertyInfo, WriteDecimal);
            }
            else if (propertyInfo.PropertyType == typeof(double))
            {
                packetReaders.Add(propertyInfo, ReadDouble);
                packetWriters.Add(propertyInfo, WriteDouble);
            }
            else if (propertyInfo.PropertyType == typeof(float))
            {
                packetReaders.Add(propertyInfo, ReadFloat);
                packetWriters.Add(propertyInfo, WriteFloat);
            }
            else if (propertyInfo.PropertyType == typeof(int))
            {
                packetReaders.Add(propertyInfo, ReadInt);
                packetWriters.Add(propertyInfo, WriteInt);
            }
            else if (propertyInfo.PropertyType == typeof(long))
            {
                packetReaders.Add(propertyInfo, ReadLong);
                packetWriters.Add(propertyInfo, WriteLong);
            }
            else if (propertyInfo.PropertyType == typeof(sbyte))
            {
                packetReaders.Add(propertyInfo, ReadSByte);
                packetWriters.Add(propertyInfo, WriteSByte);
            }
            else if (propertyInfo.PropertyType == typeof(short))
            {
                packetReaders.Add(propertyInfo, ReadShort);
                packetWriters.Add(propertyInfo, WriteShort);
            }
            else if (propertyInfo.PropertyType == typeof(string))
            {
                packetReaders.Add(propertyInfo, ReadString);
                packetWriters.Add(propertyInfo, WriteString);
            }
            else if (propertyInfo.PropertyType == typeof(uint))
            {
                packetReaders.Add(propertyInfo, ReadUInt);
                packetWriters.Add(propertyInfo, WriteUInt);
            }
            else if (propertyInfo.PropertyType == typeof(ulong))
            {
                packetReaders.Add(propertyInfo, ReadULong);
                packetWriters.Add(propertyInfo, WriteULong);
            }
            else if (propertyInfo.PropertyType == typeof(ushort))
            {
                packetReaders.Add(propertyInfo, ReadUShort);
                packetWriters.Add(propertyInfo, WriteUShort);
            }
            else if (propertyInfo.PropertyType == typeof(INetworkSerializable))
            {

            }
        }


        public virtual bool Receive(BinaryReader reader, int fromWho)
        {
            if (!PreReceive(reader, fromWho))
                return false;

            for (int i = 0; i < reflectedPropertyInfos.Count; i++)
                reflectedPropertyInfos[i].SetValue(this, packetReaders[reflectedPropertyInfos[i]](reader));

            if (Main.dedServ && (Behavior == NetworkPacketBehavior.SendToAllClients || Behavior == NetworkPacketBehavior.SendToAll))
                this.Send(fromWho, (int)NetworkDestinationType.AllClients);

            return PostReceive(reader, fromWho);
        }

        public virtual bool PostReceive(BinaryReader reader, int fromWho) => true;


        protected virtual bool PreReceive(BinaryReader reader, int fromWho) => true;


        protected virtual void PreAssignValues(ref int? fromWho, ref int? toWho) { }

        protected void AssignInitialValues(ref int? fromWho, ref int? toWho)
        {
            if (!toWho.HasValue)
            {
                if (Behavior == NetworkPacketBehavior.SendToClient)
                    throw new ArgumentNullException($"The packet behavior is defined as sending to a specific client. You must specify the client id with the {nameof(toWho)} field.");

                if (Behavior == NetworkPacketBehavior.SendToServer)
                    toWho = 256;
                else if (Behavior == NetworkPacketBehavior.SendToAllClients || Behavior == NetworkPacketBehavior.SendToAll)
                    toWho = -1;
            }
        }


        protected virtual void PrePopulatePacket(ModPacket modPacket, ref int fromWho, ref int toWho) { }

        protected virtual void PopulatePacket(ModPacket modPacket, int? fromWho, int? toWho)
        {
            for (int i = 0; i < reflectedPropertyInfos.Count; i++)
                packetWriters[reflectedPropertyInfos[i]](modPacket, reflectedPropertyInfos[i].GetValue(this));
        }


        public virtual void Send(int? fromWho = null, int? toWho = null)
        {
            if (Main.netMode == NetmodeID.SinglePlayer)
                return;

            ModPacket modPacket = MakePacket();

            PreAssignValues(ref fromWho, ref toWho);
            AssignInitialValues(ref fromWho, ref toWho);

            int
                confirmedFromWho = fromWho.Value,
                confirmedToWho = toWho.Value;

            PrePopulatePacket(modPacket, ref confirmedFromWho, ref confirmedToWho);
            PopulatePacket(modPacket, confirmedFromWho, confirmedToWho);

            modPacket.Send(confirmedToWho, confirmedFromWho);
        }


        #region Packet Writers

        private void WriteBool(ModPacket modPacket, object value) => modPacket.Write((bool)value);
        private void WriteByte(ModPacket modPacket, object value) => modPacket.Write((byte)value);
        private void WriteChar(ModPacket modPacket, object value) => modPacket.Write((char)value);
        private void WriteDecimal(ModPacket modPacket, object value) => modPacket.Write((decimal)value);
        private void WriteDouble(ModPacket modPacket, object value) => modPacket.Write((double)value);
        private void WriteFloat(ModPacket modPacket, object value) => modPacket.Write((float)value);
        private void WriteInt(ModPacket modPacket, object value) => modPacket.Write((int)value);
        private void WriteLong(ModPacket modPacket, object value) => modPacket.Write((long)value);
        private void WriteSByte(ModPacket modPacket, object value) => modPacket.Write((sbyte)value);
        private void WriteShort(ModPacket modPacket, object value) => modPacket.Write((short)value);
        private void WriteString(ModPacket modPacket, object value) => modPacket.Write((string)value);
        private void WriteUInt(ModPacket modPacket, object value) => modPacket.Write((uint)value);
        private void WriteULong(ModPacket modPacket, object value) => modPacket.Write((ulong)value);
        private void WriteUShort(ModPacket modPacket, object value) => modPacket.Write((ushort)value);
        private void WriteNetworkSerializable(ModPacket modPacket, object value) => ((INetworkSerializable)value).Send(modPacket);


        #endregion

        #region Packet Readers

        private object ReadBool(BinaryReader reader) => reader.ReadBoolean();
        private object ReadByte(BinaryReader reader) => reader.ReadByte();
        private object ReadChar(BinaryReader reader) => reader.ReadChar();
        private object ReadDecimal(BinaryReader reader) => reader.ReadDecimal();
        private object ReadDouble(BinaryReader reader) => reader.ReadDouble();
        private object ReadFloat(BinaryReader reader) => reader.ReadSingle();
        private object ReadInt(BinaryReader reader) => reader.ReadInt32();
        private object ReadLong(BinaryReader reader) => reader.ReadInt64();
        private object ReadSByte(BinaryReader reader) => reader.ReadSByte();
        private object ReadShort(BinaryReader reader) => reader.ReadInt16();
        private object ReadString(BinaryReader reader) => reader.ReadString();
        private object ReadUInt(BinaryReader reader) => reader.ReadUInt32();
        private object ReadULong(BinaryReader reader) => reader.ReadUInt64();
        private object ReadUShort(BinaryReader reader) => reader.ReadUInt16();
        private void ReadNetworkSerializable(INetworkSerializable networkSerializable, BinaryReader reader) => networkSerializable.Receive(reader);

        #endregion


        protected ModPacket MakePacket()
        {
            ModPacket packet = Mod.GetPacket();
            packet.Write(Id);

            return packet;
        }


        protected void AddAllProperties() => AddAllProperties(GetType());
        protected void AddAllProperties<T>() => AddAllProperties(typeof(T));

        protected void AddAllProperties(Type type)
        {
            foreach (PropertyInfo propertyInfo in GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                NotNetworkField notNetworkField = propertyInfo.GetCustomAttribute<NotNetworkField>();

                if (notNetworkField != null)
                    continue;

                reflectedPropertyInfos.Add(propertyInfo);
                AddReaderWriter(propertyInfo);
            }
        }

        protected void AddAllPropertiesWithAttribute<T>(Type type) where T : Attribute
        {
            foreach (PropertyInfo propertyInfo in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                T attr = propertyInfo.GetCustomAttribute<T>();

                if (attr == null)
                    continue;

                reflectedPropertyInfos.Add(propertyInfo);
                AddReaderWriter(propertyInfo);
            }
        }


        /// <summary>The <see cref="Mod"/> to which this packet belongs to. Initialized after the constructor has been called.</summary>
        [NotNetworkField]
        public Mod Mod => NetworkPacketLoader.Instance.GetMod(this);

        /// <summary>The byte type of the packet, automatically assigned after the constructor has been called.</summary>
        [NotNetworkField]
        public ushort Id => NetworkPacketLoader.Instance.GetId(GetType());

        [NotNetworkField]
        public virtual NetworkPacketBehavior Behavior => NetworkPacketBehavior.SendToAll;

        [NotNetworkField]
        public object ContextEntity { get; protected set; }
    }
}