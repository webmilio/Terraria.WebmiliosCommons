using System;
using WebmilioCommons.Networking.Packets;

namespace WebmilioCommons.Networking
{
    /// <summary>The different behaviors for network packets.</summary>
    [Flags]
    public enum NetworkPacketBehavior : byte
    {
        /// <summary>Sends directly to the specified client (toWho).</summary>
        SendToClient = 1,

        /// <summary>
        /// Sends to all clients using the server as a bounce, but <see cref="NetworkPacket.MidReceive"/> and <see cref="NetworkPacket.PostReceive"/> are not executed on the server (automatically resent).
        /// In the case of the server, the two aformentioned methods will always return <c>true</c>.</summary>
        SendToAllClients = SendToClient | 4,

        /// <summary>Sends the packet only to the server (not resent).</summary>
        SendToServer = 2,

        /// <summary>Sends to all clients using the server as a bounce (automatically resent).</summary>
        SendToAll = SendToAllClients | SendToServer
    }
}