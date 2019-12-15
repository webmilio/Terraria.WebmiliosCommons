using System.Collections.Generic;

namespace WebmilioCommons.Effects.ScreenShaking
{
    public class ScreenShake
    {
        private ScreenShake(int intensity, int duration)
        {
            Intensity = intensity;
            Duration = duration;
        }


        public int Intensity { get; }

        public int Duration { get; }


        private static List<ScreenShake> _screenShakes;


        internal static void Load() => _screenShakes = new List<ScreenShake>();
        internal static void Unload() => _screenShakes = null;


        public static ScreenShake ShakeScreen(int intensity, int duration, bool synchronize = true)
        {
            ScreenShake screenShake = new ScreenShake(intensity, duration);

            if (synchronize)
                new ScreenShakePacket(screenShake).Send();

            return screenShake;
        }

        internal static void ReceiveScreenShake(ScreenShakePacket packet)
        {
            ShakeScreen(packet.Intensity, packet.Duration, false);
        }


        public static ScreenShake[] Current => _screenShakes.ToArray();
    }
}