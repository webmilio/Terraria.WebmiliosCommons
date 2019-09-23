using System;
using WebmilioCommons.Networking;
using WebmilioCommons.Networking.Packets;

namespace WebmilioCommons.Players
{
    public sealed class WCPlayerOnJoinWorld : ModPlayerNetworkPacket<WCPlayer>
    {
        public string Guid
        {
            get => ModPlayer.UniqueID.ToString();
            set => ModPlayer.UniqueID = System.Guid.Parse(value);
        }


        // TODO Find a more secure way to handle unique identifiers.
        public override NetworkPacketBehavior Behavior { get; } = NetworkPacketBehavior.SendToAll;
    }
}