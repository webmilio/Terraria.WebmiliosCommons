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
    }
}