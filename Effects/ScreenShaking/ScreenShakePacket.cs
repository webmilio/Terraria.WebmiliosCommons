using System.IO;
using WebmilioCommons.Networking.Packets;
using WebmilioCommons.Players;

namespace WebmilioCommons.Effects.ScreenShaking
{
    public class ScreenShakePacket : ModPlayerNetworkPacket<WCPlayer>
    {
        public ScreenShakePacket()
        {
        }

        public ScreenShakePacket(ScreenShake screenShake)
        {
            Intensity = screenShake.Intensity;
            Duration = screenShake.Duration;
        }


        protected override bool PostReceive(BinaryReader reader, int fromWho)
        {
            ScreenShake.ReceiveScreenShake(this);

            return base.PostReceive(reader, fromWho);
        }


        public int Intensity { get; }

        public int Duration { get; }
    }
}