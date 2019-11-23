using System;

namespace WebmilioCommons.Effects.ScreenShaking
{
    [Obsolete("Work-in-progress")]
    public class ScreenShake
    {
        public ScreenShake(int intensity, int duration)
        {

        }


        public static ScreenShake ShakeScreen(int intensity, int duration, bool synchronize = false)
        {
            ScreenShake screenShake = new ScreenShake(intensity, duration);

            return screenShake;
        }


        public int Intensity { get; }

        public int Duration { get; }
    }
}