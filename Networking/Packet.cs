using System.Collections.ObjectModel;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using WebCom.Annotations;
using WebCom.Networking;

namespace WebCom;

public abstract class Packet
{
    public const int ServerWhoAmI = 256;

    /// <returns><c>true</c> to keep sending the packet; otherwise <c>false</c>.</returns>
    protected virtual bool PreSend(int toClient, int ignoreClient) => true;
    protected virtual void PostSend(int toClient, int ignoreClient) { }

    /// <summary>
    ///     Only override this if you know what you're doing and 
    ///     intend to add a layer of unmapped data using ModPacket writers.
    ///     If you need to add data after the mappers have written to the packet, use <see cref="SendPostMap(int, int)"/>.
    /// </summary>
    protected virtual void SendPreMap(int toClient = -1, int ignoreClient = -1) { }

    /// <summary>Override this to add unmapped data to the packet using ModPacket writers.</summary>
    protected virtual void SendPostMap(int toClient = -1, int ignoreClient = -1) { }

    /// <summary>
    ///     Sends all the information you've written between client and server. If the <paramref name="toClient"/>
    ///     parameter is non-negative, this packet will only be sent to the specified client.
    ///     If the <paramref name="ignoreClient"/> parameter is non-negative, this packet will not be sent to
    ///     the specified client.
    /// </summary>
    /// <param name="toClient"></param>
    /// <param name="ignoreClient"></param>
    public virtual void Send(int toClient = -1, int ignoreClient = -1) 
    {
        // TODO This line is tied to LocalPacketLoader@PreparePacket(ushort typeId, Type type, Packet packet)
        if (Main.netMode == NetmodeID.SinglePlayer) return;
        if (!PreSend(toClient, ignoreClient)) return;

        ModPacket.Write(PacketTypeId);
        SendPreMap(toClient, ignoreClient);

        for (int i = 0; i < Mappings.Count; i++)
        {
            Mappings[i].Serializer.Writer(this, Mappings[i].Property.GetValue(this));
        }

        SendPostMap(toClient, ignoreClient);

        ModPacket.Send(toClient, ignoreClient);
        PostSend(toClient, ignoreClient);
    }

    /// <returns><c>true</c> to keep receiving the packet; otherwise <c>false</c>.</returns>
    protected virtual bool PreReceive(BinaryReader reader, int fromWho) => true;
    protected virtual void PostReceive(BinaryReader reader, int fromWho) { }

    /// <summary>
    ///     Only override this if you know what you're doing and 
    ///     intend to add a layer of data that is not mapped (using ModPacket readers).
    /// </summary>
    protected virtual void ReceivePreMap(BinaryReader reader, int fromWho) { }

    public virtual void Receive(BinaryReader reader, int fromWho)
    { 
        if (!PreReceive(reader, fromWho)) return;

        ReceivePreMap(reader, fromWho);

        for (int i = 0; i < Mappings.Count; i++)
        {
            Mappings[i].Property.SetValue(this, Mappings[i].Serializer.Reader(this, reader));
        }

        PostReceive(reader, fromWho);
    }

    [Skip] public bool IsServer => Main.netMode == NetmodeID.Server;

    [Skip] public ushort PacketTypeId { get; internal set; }
    [Skip] public Mod Mod { get; internal set; }
    [Skip] public ModPacket ModPacket { get; internal set; }

    [Skip] public PacketMapper Mapper { get; internal set; }
    [Skip] public ReadOnlyCollection<PacketMapper.MapEntry> Mappings { get; internal set; }
}
