namespace WebmilioCommons.Networking
{
    /// <summary>The different behaviors for network packets.</summary>
    public enum NetworkPacketBehavior
    {
        /// <summary>Sends the packet directly to the specified client (toWho).</summary>
        SendToClient,

        /// <summary>Sends to all clients using the server as a bounce, but without processing said packet on the server (automatically resent).</summary>
        SendToAllClients,

        /// <summary>Sends the packet only to the server (not resent).</summary>
        SendToServer,

        /// <summary>Sends to all clients using the server as a bounce (automatically resent).</summary>
        SendToAll
    }
}