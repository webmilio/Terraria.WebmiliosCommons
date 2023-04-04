using System.Collections.Generic;
using WebCom.Networking;

namespace WebCom.Effects.ScreenShaking
{
    public class ScreenShake
    {
        private ScreenShake(int intensity, int duration, bool slowsDown)
        {
            Intensity = intensity;
            Duration = duration;
            SlowsDown = slowsDown;
        }

        public int Intensity { get; set; }
        public int Duration { get; set; }
        public bool SlowsDown { get; }

        private static List<ScreenShake> _screenShakes = new List<ScreenShake>();

        internal static void Load() => _screenShakes = new List<ScreenShake>();
        internal static void Unload() => _screenShakes = null;


        public static ScreenShake ShakeScreen(int intensity, int duration, bool slowsDown = true, bool synchronize = true)
        {
            ScreenShake screenShake = new ScreenShake(intensity, duration, slowsDown);

            if (synchronize)
                WebComMod.This.PreparePacket(new ScreenShakePacket(screenShake)).Send();

            _screenShakes.Add(screenShake);
            return screenShake;
        }

        internal static void ReceiveScreenShake(ScreenShakePacket packet)
        {
            ShakeScreen(packet.Intensity, packet.Duration, packet.SlowsDown, false);
        }

        internal static void TickScreenShakes()
        {
            foreach (ScreenShake screenShake in _screenShakes)
            {
                screenShake.Duration--;

                if (screenShake.SlowsDown && screenShake.Intensity > 0)
                    screenShake.Intensity--;
            }

            _screenShakes.RemoveAll(s => s.Duration <= 0);
        }

        public static ScreenShake[] Current => _screenShakes.ToArray();
    }
}