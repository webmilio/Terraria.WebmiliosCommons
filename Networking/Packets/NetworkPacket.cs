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
        [NotNetworkField]
        internal static Dictionary<Type, List<PropertyInfo>> GlobalReflectedPropertyInfos { get; set; }

        [NotNetworkField]
        internal static Dictionary<Type, Dictionary<PropertyInfo, Action<ModPacket, object>>> GlobalPacketWriters { get; set; }
        [NotNetworkField]
        internal static Dictionary<Type, Dictionary<PropertyInfo, Func<NetworkPacket, BinaryReader, object>>> GlobalPacketReaders { get; set; }


        internal static void Initialize()
        {
            GlobalReflectedPropertyInfos = new Dictionary<Type, List<PropertyInfo>>();

            GlobalPacketWriters = new Dictionary<Type, Dictionary<PropertyInfo, Action<ModPacket, object>>>();
            GlobalPacketReaders = new Dictionary<Type, Dictionary<PropertyInfo, Func<NetworkPacket, BinaryReader, object>>>();
        }

        internal static void Unload()
        {

            GlobalReflectedPropertyInfos = null;

            GlobalPacketWriters = null;
            GlobalPacketReaders = null;
        }


        protected NetworkPacket() : this(true) { }

        protected NetworkPacket(bool autoGetProperties)
        {
            if (autoGetProperties)
                AddAllProperties();
        }


        private void AddReaderWriter(PropertyInfo propertyInfo)
        {
            if (propertyInfo.PropertyType == typeof(bool))
            {
                PacketReaders.Add(propertyInfo, ReadBool);
                PacketWriters.Add(propertyInfo, WriteBool);
            }
            else if (propertyInfo.PropertyType == typeof(byte))
            {
                PacketReaders.Add(propertyInfo, ReadByte);
                PacketWriters.Add(propertyInfo, WriteByte);
            }
            else if (propertyInfo.PropertyType == typeof(char))
            {
                PacketReaders.Add(propertyInfo, ReadChar);
                PacketWriters.Add(propertyInfo, WriteChar);
            }
            else if (propertyInfo.PropertyType == typeof(decimal))
            {
                PacketReaders.Add(propertyInfo, ReadDecimal);
                PacketWriters.Add(propertyInfo, WriteDecimal);
            }
            else if (propertyInfo.PropertyType == typeof(double))
            {
                PacketReaders.Add(propertyInfo, ReadDouble);
                PacketWriters.Add(propertyInfo, WriteDouble);
            }
            else if (propertyInfo.PropertyType == typeof(float))
            {
                PacketReaders.Add(propertyInfo, ReadFloat);
                PacketWriters.Add(propertyInfo, WriteFloat);
            }
            else if (propertyInfo.PropertyType == typeof(int))
            {
                PacketReaders.Add(propertyInfo, ReadInt);
                PacketWriters.Add(propertyInfo, WriteInt);
            }
            else if (propertyInfo.PropertyType == typeof(long))
            {
                PacketReaders.Add(propertyInfo, ReadLong);
                PacketWriters.Add(propertyInfo, WriteLong);
            }
            else if (propertyInfo.PropertyType == typeof(sbyte))
            {
                PacketReaders.Add(propertyInfo, ReadSByte);
                PacketWriters.Add(propertyInfo, WriteSByte);
            }
            else if (propertyInfo.PropertyType == typeof(short))
            {
                PacketReaders.Add(propertyInfo, ReadShort);
                PacketWriters.Add(propertyInfo, WriteShort);
            }
            else if (propertyInfo.PropertyType == typeof(string))
            {
                PacketReaders.Add(propertyInfo, ReadString);
                PacketWriters.Add(propertyInfo, WriteString);
            }
            else if (propertyInfo.PropertyType == typeof(uint))
            {
                PacketReaders.Add(propertyInfo, ReadUInt);
                PacketWriters.Add(propertyInfo, WriteUInt);
            }
            else if (propertyInfo.PropertyType == typeof(ulong))
            {
                PacketReaders.Add(propertyInfo, ReadULong);
                PacketWriters.Add(propertyInfo, WriteULong);
            }
            else if (propertyInfo.PropertyType == typeof(ushort))
            {
                PacketReaders.Add(propertyInfo, ReadUShort);
                PacketWriters.Add(propertyInfo, WriteUShort);
            }
            else
            {
                Type[] interfaceTypes = propertyInfo.PropertyType.GetInterfaces();

                for (int i = 0; i < interfaceTypes.Length; i++)
                    if (interfaceTypes[i] == typeof(INetworkSerializable))
                    {
                        PacketReaders.Add(propertyInfo, (networkPacket, reader) =>
                        {
                            object value = propertyInfo.GetValue(networkPacket);

                            if (value == null)
                                throw new NullReferenceException($"Tried obtaining instance of {nameof(INetworkSerializable)} from {propertyInfo.Name}: obtained null.");

                            INetworkSerializable networkSerializable = (INetworkSerializable)value;

                            ReadNetworkSerializable(networkPacket, networkSerializable, reader);
                            return networkSerializable;
                        });

                        PacketWriters.Add(propertyInfo, WriteNetworkSerializable);
                    }
            }
        }


        /// <summary>
        /// The entire logic for receiving a packet (including resending from the server) is in this method.
        /// It is better to override PreReceive or PostReceive, depending on the behavior you want.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="fromWho"></param>
        /// <returns></returns>
        public virtual bool Receive(BinaryReader reader, int fromWho)
        {
            if (!PreReceive(reader, fromWho))
                return false;

            foreach (PropertyInfo propertyInfo in ReflectedPropertyInfos)
                propertyInfo.SetValue(this, PacketReaders[propertyInfo](this, reader));

            if (!MidReceive(reader, fromWho))
                return false;

            if (Main.dedServ && (Behavior == NetworkPacketBehavior.SendToAllClients || Behavior == NetworkPacketBehavior.SendToAll))
                this.Send(fromWho, (int)NetworkDestinationType.AllClients);

            return PostReceive(reader, fromWho);
        }

        protected virtual bool PreReceive(BinaryReader reader, int fromWho) => true;

        /// <summary>Called before the packet is resent (in cases where it should). Useful for defining custom behavior that needs to be replicated on all clients.</summary>
        /// <param name="reader"></param>
        /// <param name="fromWho"></param>
        /// <returns></returns>
        protected virtual bool MidReceive(BinaryReader reader, int fromWho) => true;

        protected virtual bool PostReceive(BinaryReader reader, int fromWho) => true;


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

            if (!fromWho.HasValue)
                fromWho = Main.myPlayer;
        }


        protected virtual void PrePopulatePacket(ModPacket modPacket, ref int fromWho, ref int toWho) { }

        protected virtual void PopulatePacket(ModPacket modPacket, int? fromWho, int? toWho)
        {
            foreach (PropertyInfo propertyInfo in ReflectedPropertyInfos)
                PacketWriters[propertyInfo](modPacket, propertyInfo.GetValue(this));
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

            if (!PreSend(modPacket, fromWho, toWho))
                return;

            modPacket.Send(confirmedToWho, confirmedFromWho);
            NetworkPacketLoader.OnPacketSent(this);

            PostSend(modPacket, fromWho, toWho);
        }

        protected virtual bool PreSend(ModPacket modPacket, int? fromWho = null, int? toWho = null) => true;

        protected virtual void PostSend(ModPacket modPacket, int? fromWho = null, int? toWho = null) { }


        #region Packet Writers

        public void WriteBool(ModPacket modPacket, object value) => modPacket.Write((bool)value);
        public void WriteByte(ModPacket modPacket, object value) => modPacket.Write((byte)value);
        public void WriteChar(ModPacket modPacket, object value) => modPacket.Write((char)value);
        public void WriteDecimal(ModPacket modPacket, object value) => modPacket.Write((decimal)value);
        public void WriteDouble(ModPacket modPacket, object value) => modPacket.Write((double)value);
        public void WriteFloat(ModPacket modPacket, object value) => modPacket.Write((float)value);
        public void WriteInt(ModPacket modPacket, object value) => modPacket.Write((int)value);
        public void WriteLong(ModPacket modPacket, object value) => modPacket.Write((long)value);
        public void WriteSByte(ModPacket modPacket, object value) => modPacket.Write((sbyte)value);
        public void WriteShort(ModPacket modPacket, object value) => modPacket.Write((short)value);
        public void WriteString(ModPacket modPacket, object value) => modPacket.Write((string)value);
        public void WriteUInt(ModPacket modPacket, object value) => modPacket.Write((uint)value);
        public void WriteULong(ModPacket modPacket, object value) => modPacket.Write((ulong)value);
        public void WriteUShort(ModPacket modPacket, object value) => modPacket.Write((ushort)value);
        public void WriteNetworkSerializable(ModPacket modPacket, object value) => ((INetworkSerializable)value).Send(this, modPacket);


        #endregion

        #region Packet Readers

        public object ReadBool(NetworkPacket networkPacket, BinaryReader reader) => reader.ReadBoolean();
        public object ReadByte(NetworkPacket networkPacket, BinaryReader reader) => reader.ReadByte();
        public object ReadChar(NetworkPacket networkPacket, BinaryReader reader) => reader.ReadChar();
        public object ReadDecimal(NetworkPacket networkPacket, BinaryReader reader) => reader.ReadDecimal();
        public object ReadDouble(NetworkPacket networkPacket, BinaryReader reader) => reader.ReadDouble();
        public object ReadFloat(NetworkPacket networkPacket, BinaryReader reader) => reader.ReadSingle();
        public object ReadInt(NetworkPacket networkPacket, BinaryReader reader) => reader.ReadInt32();
        public object ReadLong(NetworkPacket networkPacket, BinaryReader reader) => reader.ReadInt64();
        public object ReadSByte(NetworkPacket networkPacket, BinaryReader reader) => reader.ReadSByte();
        public object ReadShort(NetworkPacket networkPacket, BinaryReader reader) => reader.ReadInt16();
        public object ReadString(NetworkPacket networkPacket, BinaryReader reader) => reader.ReadString();
        public object ReadUInt(NetworkPacket networkPacket, BinaryReader reader) => reader.ReadUInt32();
        public object ReadULong(NetworkPacket networkPacket, BinaryReader reader) => reader.ReadUInt64();
        public object ReadUShort(NetworkPacket networkPacket, BinaryReader reader) => reader.ReadUInt16();
        public void ReadNetworkSerializable(NetworkPacket networkPacket, INetworkSerializable networkSerializable, BinaryReader reader) => networkSerializable.Receive(networkPacket, reader);

        #endregion

        /// <summary>Standard packet instance creation.</summary>
        /// <returns>An instance of <see cref="ModPacket"/> with an auto-generated ID.</returns>
        protected ModPacket MakePacket()
        {
            ModPacket packet = Mod.GetPacket();
            packet.Write(Id);

            return packet;
        }

        /// <summary>Parses the current class's properties into the cache for sending and receiving.</summary>
        protected void AddAllProperties() => AddAllProperties(GetType());

        protected void AddAllProperties(Type type)
        {
            if (GlobalReflectedPropertyInfos.ContainsKey(type))
                return;

            List<PropertyInfo> propertyInfos = AddReflectedType(type);

            foreach (PropertyInfo propertyInfo in GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                NotNetworkField notNetworkField = propertyInfo.GetCustomAttribute<NotNetworkField>();

                if (notNetworkField != null)
                    continue;

                propertyInfos.Add(propertyInfo);
                AddReaderWriter(propertyInfo);
            }
        }

        /*protected void AddAllPropertiesWithAttribute<T>(Type type) where T : Attribute
        {
            foreach (PropertyInfo propertyInfo in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                T attr = propertyInfo.GetCustomAttribute<T>();

                if (attr == null)
                    continue;

                ReflectedPropertyInfos.Add(propertyInfo);
                AddReaderWriter(propertyInfo);
            }
        }*/


        private static List<PropertyInfo> AddReflectedType(Type type)
        {
            GlobalReflectedPropertyInfos.Add(type, new List<PropertyInfo>());

            GlobalPacketWriters.Add(type, new Dictionary<PropertyInfo, Action<ModPacket, object>>());
            GlobalPacketReaders.Add(type, new Dictionary<PropertyInfo, Func<NetworkPacket, BinaryReader, object>>());

            return GlobalReflectedPropertyInfos[type];
        }


        /// <summary>The <see cref="Mod"/> to which this packet belongs to. Initialized after the constructor has been called.</summary>
        [NotNetworkField]
        public Mod Mod => NetworkPacketLoader.Instance.GetMod(this);

        /// <summary>The ushort type of the packet, automatically assigned after the constructor has been called.</summary>
        [NotNetworkField]
        public int Id => NetworkPacketLoader.Instance.GetId(GetType());

        [NotNetworkField]
        public virtual NetworkPacketBehavior Behavior => NetworkPacketBehavior.SendToAll;

        [NotNetworkField]
        public object ContextEntity { get; protected set; }


        [NotNetworkField]
        public List<PropertyInfo> ReflectedPropertyInfos => GlobalReflectedPropertyInfos[GetType()];

        [NotNetworkField]
        public Dictionary<PropertyInfo, Action<ModPacket, object>> PacketWriters => GlobalPacketWriters[GetType()];

        [NotNetworkField]
        public Dictionary<PropertyInfo, Func<NetworkPacket, BinaryReader, object>> PacketReaders => GlobalPacketReaders[GetType()];
    }
}