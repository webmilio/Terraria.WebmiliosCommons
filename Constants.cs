using System;

namespace WebmilioCommons
{
    public static class Constants
    {
        [Obsolete("Use " + nameof(TicksPerSecond) + ".", true)]
        public const int TICKS_PER_SECOND = TicksPerSecond;
        public const int 
            TicksPerSecond = 60,
            
            DayBeginTime = 0,
            DayEndTime = 54000,
            
            NightBeginTime = 0,
            NightEndTime = 32400;
    }
}