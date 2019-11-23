using WebmilioCommons.Networking.Packets;
using WebmilioCommons.Players;

namespace WebmilioCommons.Effects.ScreenShaking
{
    public class ScreenShakePacket : ModPlayerNetworkPacket<WCPlayer>
    {
        public ScreenShakePacket()
        {
        }

        public ScreenShakePacket(int duration, int intensity)
        {
            
        }


        public int Intensity { get; }

        public int Duration { get; }
    }
}