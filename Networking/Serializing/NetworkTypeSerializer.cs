using System;
using System.IO;
using Terraria.ModLoader;
using WebmilioCommons.Networking.Packets;

namespace WebmilioCommons.Networking.Serializing
{
    public class NetworkTypeSerializer
    {
        public NetworkTypeSerializer(Func<NetworkPacket, BinaryReader, object> reader, Action<NetworkPacket, ModPacket, object> writer)
        {
            Reader = reader;
            Writer = writer;
        }


        public Func<NetworkPacket, BinaryReader, object> Reader { get; }

        public Action<NetworkPacket, ModPacket, object> Writer { get; }
    }
}