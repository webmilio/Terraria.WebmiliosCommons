using System.IO;
using Terraria;

namespace WebCom.Effects.ScreenShaking
{
    public class ScreenShakePacket : Networking.Packet
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

        protected override void PostReceive(BinaryReader reader, int fromWho)
        {
            if (!Main.dedServ)
                ScreenShake.ReceiveScreenShake(this);
        }


        public int Intensity { get; }

        public int Duration { get; }

        public bool SlowsDown { get; }
    }
}