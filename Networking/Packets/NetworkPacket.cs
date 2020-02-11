﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using WebmilioCommons.Networking.Attributes;

namespace WebmilioCommons.Networking.Packets
{
    public abstract class NetworkPacket
    {
        protected NetworkPacket() : this(true) { }

        protected NetworkPacket(bool autoGetProperties)
        {
            if (autoGetProperties)
                AddAllProperties();
        }


        /// <summary>Parses the current class's properties into the cache for sending and receiving.</summary>
        protected void AddAllProperties() => AddAllProperties(GetType());

        protected void AddAllProperties(Type type)
        {
            if (NetworkPacketReflectionCache.globalReflectedPropertyInfos.ContainsKey(type))
                return;

            AutoNetworkMappingAttribute mappingAttribute = type.GetCustomAttribute<AutoNetworkMappingAttribute>();
            AutoNetworkMappingBehavior mappingBehavior = AutoNetworkMappingBehavior.OptOut;

            if (mappingAttribute != default)
                mappingBehavior = mappingAttribute.Behavior;

            if (mappingBehavior == AutoNetworkMappingBehavior.DoNotMap)
                return;


            List<PropertyInfo> propertyInfos = NetworkPacketReflectionCache.AddReflectedType(type);

            foreach (PropertyInfo propertyInfo in GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                NotNetworkFieldAttribute nnfAttribute = propertyInfo.GetCustomAttribute<NotNetworkFieldAttribute>();

                if (nnfAttribute != default && mappingBehavior == AutoNetworkMappingBehavior.OptOut)
                    continue;


                NetworkFieldAttribute nfAttribute = propertyInfo.GetCustomAttribute<NetworkFieldAttribute>();

                if (mappingBehavior == AutoNetworkMappingBehavior.OptIn && nfAttribute == default)
                    continue;


                propertyInfos.Add(propertyInfo);
                AddReaderWriter(propertyInfo);
            }
        }

        private void AddReaderWriter(PropertyInfo propertyInfo)
        {
            if (propertyInfo.PropertyType == typeof(bool))
            {
                PacketReaders.Add(propertyInfo, NetworkPacketIOExtensions.ReadBool);
                PacketWriters.Add(propertyInfo, NetworkPacketIOExtensions.WriteBool);
            }
            else if (propertyInfo.PropertyType == typeof(byte))
            {
                PacketReaders.Add(propertyInfo, NetworkPacketIOExtensions.ReadByte);
                PacketWriters.Add(propertyInfo, NetworkPacketIOExtensions.WriteByte);
            }
            else if (propertyInfo.PropertyType == typeof(char))
            {
                PacketReaders.Add(propertyInfo, NetworkPacketIOExtensions.ReadChar);
                PacketWriters.Add(propertyInfo, NetworkPacketIOExtensions.WriteChar);
            }
            else if (propertyInfo.PropertyType == typeof(decimal))
            {
                PacketReaders.Add(propertyInfo, NetworkPacketIOExtensions.ReadDecimal);
                PacketWriters.Add(propertyInfo, NetworkPacketIOExtensions.WriteDecimal);
            }
            else if (propertyInfo.PropertyType == typeof(double))
            {
                PacketReaders.Add(propertyInfo, NetworkPacketIOExtensions.ReadDouble);
                PacketWriters.Add(propertyInfo, NetworkPacketIOExtensions.WriteDouble);
            }
            else if (propertyInfo.PropertyType == typeof(float))
            {
                PacketReaders.Add(propertyInfo, NetworkPacketIOExtensions.ReadFloat);
                PacketWriters.Add(propertyInfo, NetworkPacketIOExtensions.WriteFloat);
            }
            else if (propertyInfo.PropertyType == typeof(int))
            {
                PacketReaders.Add(propertyInfo, NetworkPacketIOExtensions.ReadInt);
                PacketWriters.Add(propertyInfo, NetworkPacketIOExtensions.WriteInt);
            }
            else if (propertyInfo.PropertyType == typeof(long))
            {
                PacketReaders.Add(propertyInfo, NetworkPacketIOExtensions.ReadLong);
                PacketWriters.Add(propertyInfo, NetworkPacketIOExtensions.WriteLong);
            }
            else if (propertyInfo.PropertyType == typeof(sbyte))
            {
                PacketReaders.Add(propertyInfo, NetworkPacketIOExtensions.ReadSByte);
                PacketWriters.Add(propertyInfo, NetworkPacketIOExtensions.WriteSByte);
            }
            else if (propertyInfo.PropertyType == typeof(short))
            {
                PacketReaders.Add(propertyInfo, NetworkPacketIOExtensions.ReadShort);
                PacketWriters.Add(propertyInfo, NetworkPacketIOExtensions.WriteShort);
            }
            else if (propertyInfo.PropertyType == typeof(string))
            {
                PacketReaders.Add(propertyInfo, NetworkPacketIOExtensions.ReadString);
                PacketWriters.Add(propertyInfo, NetworkPacketIOExtensions.WriteString);
            }
            else if (propertyInfo.PropertyType == typeof(uint))
            {
                PacketReaders.Add(propertyInfo, NetworkPacketIOExtensions.ReadUInt);
                PacketWriters.Add(propertyInfo, NetworkPacketIOExtensions.WriteUInt);
            }
            else if (propertyInfo.PropertyType == typeof(ulong))
            {
                PacketReaders.Add(propertyInfo, NetworkPacketIOExtensions.ReadULong);
                PacketWriters.Add(propertyInfo, NetworkPacketIOExtensions.WriteULong);
            }
            else if (propertyInfo.PropertyType == typeof(ushort))
            {
                PacketReaders.Add(propertyInfo, NetworkPacketIOExtensions.ReadUShort);
                PacketWriters.Add(propertyInfo, NetworkPacketIOExtensions.WriteUShort);
            }
            else if (propertyInfo.PropertyType == typeof(Item))
            {
                PacketReaders.Add(propertyInfo, NetworkPacketIOExtensions.ReadItem);
                PacketWriters.Add(propertyInfo, NetworkPacketIOExtensions.WriteItem);
            }
            else if (propertyInfo.PropertyType == typeof(Vector2))
            {
                PacketReaders.Add(propertyInfo, NetworkPacketIOExtensions.ReadVector2);
                PacketWriters.Add(propertyInfo, NetworkPacketIOExtensions.WriteVector2);
            }
            else if (propertyInfo.PropertyType == typeof(Color))
            {
                PacketReaders.Add(propertyInfo, NetworkPacketIOExtensions.ReadRGB);
                PacketWriters.Add(propertyInfo, NetworkPacketIOExtensions.WriteRGB);
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

                            networkPacket.ReadNetworkSerializable(networkSerializable, reader);
                            return networkSerializable;
                        });

                        PacketWriters.Add(propertyInfo, NetworkPacketIOExtensions.WriteNetworkSerializable);
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

            if (Main.netMode == NetmodeID.Server && (Behavior == NetworkPacketBehavior.SendToAllClients || Behavior == NetworkPacketBehavior.SendToAll))
                this.Send(fromWho, (int)NetworkDestinationType.AllClients);

            return PostReceive(reader, fromWho);
        }

        // TODO CHANGE THIS TO DOPRERECEIVE.
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
                PacketWriters[propertyInfo](this, modPacket, propertyInfo.GetValue(this));
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


        /// <summary>Standard packet instance creation.</summary>
        /// <returns>An instance of <see cref="ModPacket"/> with an auto-generated ID.</returns>
        protected ModPacket MakePacket()
        {
            ModPacket packet = Mod.GetPacket();
            NetworkPacketLoader.Instance.PacketIdWriter(this, packet, Id);

            return packet;
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
        public List<PropertyInfo> ReflectedPropertyInfos => NetworkPacketReflectionCache.globalReflectedPropertyInfos[GetType()];

        [NotNetworkField]
        public Dictionary<PropertyInfo, Action<NetworkPacket, ModPacket, object>> PacketWriters => NetworkPacketReflectionCache.globalPacketWriters[GetType()];

        [NotNetworkField]
        public Dictionary<PropertyInfo, Func<NetworkPacket, BinaryReader, object>> PacketReaders => NetworkPacketReflectionCache.globalPacketReaders[GetType()];
    }
}