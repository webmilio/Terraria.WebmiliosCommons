using System;
using Terraria;

namespace WebmilioCommons.Players
{
    public sealed partial class WCPlayer
    {
        public static void EnableForcefulLifeCrystalLimiting(int limit = 15)
        {
            LifeCrystalLimit = Math.Max(LifeCrystalLimit, limit);
            LifeCrystalLimiting = true;
        }



        public float HeartContainersPercentage => (float)player.statLife / player.statLifeMax2;
        public float SupposedHeartContainersPercentage => (LifeCrystalsConsumed + 5f) / (LifeCrystalLimit + 5);


        public int LifeCrystalsConsumed { get; internal set; }
        public static bool LifeCrystalLimiting { get; private set; }
        public static int LifeCrystalLimit { get; private set; }

        public bool HasReachedLifeCrystalLimit => LifeCrystalsConsumed >= LifeCrystalLimit;
    }
}
