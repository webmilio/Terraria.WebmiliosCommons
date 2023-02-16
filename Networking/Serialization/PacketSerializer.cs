using System;
using System.IO;

namespace WebCom.Networking.Serialization;

/// <summary>Provides the methods for a specific datatype to be handled when received or sent.</summary>
public class PacketSerializer
{
    public PacketSerializer(Func<Packet, BinaryReader, object> reader, Action<Packet, object> writer)
    {
        Reader = reader;
        Writer = writer;
    }

    public Func<Packet, BinaryReader, object> Reader { get; }

    public Action<Packet, object> Writer { get; }
}
