using System;
using System.IO;
using WebmilioCommons.Networking.Packets;

namespace WebmilioCommons.Players
{
    public sealed class WCPlayerOnJoinWorld : ModPlayerNetworkPacket<WCPlayer>
    {
        public WCPlayerOnJoinWorld() { }

        public WCPlayerOnJoinWorld(WCPlayer wcPlayer) : base(wcPlayer)
        {
        }


        public string Guid
        {
            get => ModPlayer.UniqueId.ToString();
            set => ModPlayer.UniqueId = System.Guid.Parse(value);
        }
    }
}