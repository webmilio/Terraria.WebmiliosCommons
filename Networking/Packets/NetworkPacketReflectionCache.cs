using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Terraria.ModLoader;
using WebmilioCommons.Networking.Attributes;

namespace WebmilioCommons.Networking.Packets
{
    public static class NetworkPacketReflectionCache
    {
        // Making the following into properties would add a bit of overhead to our calls, and these are used too often to allow that.
        internal static Dictionary<Type, List<PropertyInfo>> globalReflectedPropertyInfos;

        internal static Dictionary<Type, Dictionary<PropertyInfo, Action<NetworkPacket, ModPacket, object>>> globalPacketWriters;
        internal static Dictionary<Type, Dictionary<PropertyInfo, Func<NetworkPacket, BinaryReader, object>>> globalPacketReaders;

        internal static Dictionary<Type, Tuple<Action<NetworkPacket, ModPacket, object>, Func<NetworkPacket, BinaryReader, object>>> readersWriters;


        internal static void Initialize()
        {
            globalReflectedPropertyInfos = new Dictionary<Type, List<PropertyInfo>>();

            globalPacketWriters = new Dictionary<Type, Dictionary<PropertyInfo, Action<NetworkPacket, ModPacket, object>>>();
            globalPacketReaders = new Dictionary<Type, Dictionary<PropertyInfo, Func<NetworkPacket, BinaryReader, object>>>();
        }

        internal static void Unload()
        {
            globalReflectedPropertyInfos = null;

            globalPacketWriters = null;
            globalPacketReaders = null;
        }


        public static List<PropertyInfo> AddReflectedType(Type type)
        {
            globalReflectedPropertyInfos.Add(type, new List<PropertyInfo>());

            globalPacketWriters.Add(type, new Dictionary<PropertyInfo, Action<NetworkPacket, ModPacket, object>>());
            globalPacketReaders.Add(type, new Dictionary<PropertyInfo, Func<NetworkPacket, BinaryReader, object>>());

            return globalReflectedPropertyInfos[type];
        }
    }
}