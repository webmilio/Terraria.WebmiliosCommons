using System.IO;
using WebmilioCommons.Networking.Packets;
using WebmilioCommons.Players;

namespace WebmilioCommons.Time
{
    public sealed class TimeAlteredPacket : ModPlayerNetworkPacket<WCPlayer>
    {
        private TimeAlterationRequest.Sources _source;


        public override bool PostReceive(BinaryReader reader, int fromWho)
        {
            if (Stopped)
                TimeManagement.TryStopTime(ModPlayer, Duration, false);
            else
                TimeManagement.TryResumeTime(ModPlayer, false);

            return true;
        }


        public string Source
        {
            get => _source.ToString();
            set => TimeAlterationRequest.Sources.TryParse(value, true, out _source);
        }

        public int SourceEntity { get; set; }


        public int TickRate { get; set; }

        public int Duration { get; set; }
    }
}