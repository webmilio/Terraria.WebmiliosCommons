using System.IO;
using Terraria;
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
            SlowsDown = screenShake.SlowsDown;
        }


        protected override bool PostReceive(BinaryReader reader, int fromWho)
        {
            if (!Main.dedServ)
                ScreenShake.ReceiveScreenShake(this);

            return base.PostReceive(reader, fromWho);
        }


        public int Intensity { get; }

        public int Duration { get; }

        public bool SlowsDown { get; }
    }
}