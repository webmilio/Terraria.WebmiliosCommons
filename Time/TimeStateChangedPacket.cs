using System.IO;
using WebmilioCommons.Networking.Packets;
using WebmilioCommons.Players;

namespace WebmilioCommons.Time
{
    public sealed class TimeStateChangedPacket : ModPlayerNetworkPacket<WCPlayer>
    {
        public override bool PostReceive(BinaryReader reader, int fromWho)
        {
            if (Stopped)
                TimeManagement.TryStopTime(ModPlayer, Duration, false);
            else
                TimeManagement.TryResumeTime(ModPlayer, false);

            return true;
        }


        public bool Stopped { get; set; }

        public int Duration { get; set; }
    }
}