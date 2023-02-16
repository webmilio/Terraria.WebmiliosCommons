using System;

namespace WebCom
{
    public static class Constants
    {
        public const int
            TicksPerSecond = 60,

            DayBeginTime = 0,
            DayEndTime = 54000,

            NightBeginTime = 0,
            NightEndTime = 32400;

        /// <summary>In Tiles/Seconds^2</summary>
        public const int GravityInTiles = 30;

        /// <summary>In Meters/Seconds^2</summary>
        public const float GravityInMeters = 18.28f;
    }
}