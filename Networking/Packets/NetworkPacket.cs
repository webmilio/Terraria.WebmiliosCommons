using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using WebmilioCommons.Networking.Attributes;
using WebmilioCommons.Networking.Serializing;

namespace WebmilioCommons.Networking.Packets
{
    public abstract class NetworkPacket
    {
        [NotNetworkField]
        internal static Dictionary<Type, List<PropertyInfo>> GlobalReflectedPropertyInfos { get; set; }

        [NotNetworkField]
        internal static Dictionary<Type, Dictionary<PropertyInfo, Action<NetworkPacket, ModPacket, object>>> GlobalPacketWriters { get; set; }
        [NotNetworkField]
        internal static Dictionary<Type, Dictionary<PropertyInfo, Func<NetworkPacket, BinaryReader, object>>> GlobalPacketReaders { get; set; }


        internal static void Initialize()
        {
            GlobalReflectedPropertyInfos = new Dictionary<Type, List<PropertyInfo>>();

            GlobalPacketWriters = new Dictionary<Type, Dictionary<PropertyInfo, Action<NetworkPacket, ModPacket, object>>>();
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
            if (Loader.HasSerializer(propertyInfo.PropertyType))
            {
                NetworkTypeSerializer serializer = Loader.GetSerializer(propertyInfo.PropertyType);

                PacketReaders.Add(propertyInfo, serializer.Reader);
                PacketWriters.Add(propertyInfo, serializer.Writer);
            }
            else
            {
                Type[] interfaceTypes = propertyInfo.PropertyType.GetInterfaces();

                for (int i = 0; i < interfaceTypes.Length; i++)
                    if (interfaceTypes[i] == typeof(Serializing.INetworkSerializable))
                    {
                        PacketReaders.Add(propertyInfo, (networkPacket, reader) =>
                        {
                            object value = propertyInfo.GetValue(networkPacket);

                            if (value == null)
                                throw new NullReferenceException($"Tried obtaining instance of {nameof(Serializing.INetworkSerializable)} from {propertyInfo.Name}: obtained null.");

                            Serializing.INetworkSerializable networkSerializable = (Serializing.INetworkSerializable)value;

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
            if (!DoPreReceive(reader, fromWho) || !PreReceive(reader, fromWho))
                return false;

            foreach (PropertyInfo propertyInfo in ReflectedPropertyInfos)
                propertyInfo.SetValue(this, PacketReaders[propertyInfo](this, reader));

            if (!MidReceive(reader, fromWho))
                return false;

            if (Main.netMode == NetmodeID.Server && (Behavior == NetworkPacketBehavior.SendToAllClients || Behavior == NetworkPacketBehavior.SendToAll))
                this.Send(fromWho, (int)NetworkDestinationType.AllClients);

            return PostReceive(reader, fromWho);
        }
        
        internal virtual bool DoPreReceive(BinaryReader reader, int fromWho) => true;

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

        /// <summary>Parses the current class's properties into the cache for sending and receiving.</summary>
        protected void AddAllProperties() => AddAllProperties(GetType());

        protected void AddAllProperties(Type type)
        {
            if (GlobalReflectedPropertyInfos.ContainsKey(type))
                return;

            AutoNetworkMappingAttribute mappingAttribute = type.GetCustomAttribute<AutoNetworkMappingAttribute>();
            AutoNetworkMappingBehavior mappingBehavior = AutoNetworkMappingBehavior.OptOut;

            if (mappingAttribute != default)
                mappingBehavior = mappingAttribute.Behavior;

            if (mappingBehavior == AutoNetworkMappingBehavior.DoNotMap)
                return;


            List<PropertyInfo> propertyInfos = AddReflectedType(type);

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

            GlobalPacketWriters.Add(type, new Dictionary<PropertyInfo, Action<NetworkPacket, ModPacket, object>>());
            GlobalPacketReaders.Add(type, new Dictionary<PropertyInfo, Func<NetworkPacket, BinaryReader, object>>());

            return GlobalReflectedPropertyInfos[type];
        }


        #region Not Synchronized

        /// <summary>The <see cref="Mod"/> to which this packet belongs to. Initialized after the constructor has been called.</summary>
        [NotNetworkField]
        public Mod Mod => Loader.GetMod(this);

        /// <summary>The ushort type of the packet, automatically assigned after the constructor has been called.</summary>
        [NotNetworkField]
        public int Id => Loader.GetId(GetType());

        [NotNetworkField]
        public virtual NetworkPacketBehavior Behavior => NetworkPacketBehavior.SendToAll;

        [NotNetworkField]
        public object ContextEntity { get; protected set; }


        [NotNetworkField]
        public List<PropertyInfo> ReflectedPropertyInfos => GlobalReflectedPropertyInfos[GetType()];

        [NotNetworkField]
        public Dictionary<PropertyInfo, Action<NetworkPacket, ModPacket, object>> PacketWriters => GlobalPacketWriters[GetType()];

        [NotNetworkField]
        public Dictionary<PropertyInfo, Func<NetworkPacket, BinaryReader, object>> PacketReaders => GlobalPacketReaders[GetType()];


        [NotNetworkField]
        public NetworkPacketLoader Loader => NetworkPacketLoader.Instance;

        #endregion
    }
}