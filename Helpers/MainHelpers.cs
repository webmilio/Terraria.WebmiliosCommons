using Terraria;

namespace WebmilioCommons.Helpers;

public static class MainHelpers
{
    public const int Begin = 0,
        NightDuration = 32_400,
        DayDuration = 54_000,

        NightBegin = 0,
        NightEnd = (NightDuration / 2),

        DayBegin = NightEnd + 1,
        DayEnd = DayBegin + DayDuration,
        
        CompleteEnd = DayEnd + NightEnd;

    public static double ZeroedTime
    {
        get
        {
            var time = Main.time;

            if (Main.dayTime)
            {
                time += DayBegin;
            }
            else
            {
                if (time > NightEnd)
                {
                    time -= NightEnd;
                }
                else
                {
                    time += DayEnd;
                }
            }

            return time;
        }
    }
}