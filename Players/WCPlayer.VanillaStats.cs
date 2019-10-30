using Terraria;

namespace WebmilioCommons.Players
{
    public sealed partial class WCPlayer
    {
        public static void EnableForcefulLifeCrystalLimiting()
        {
            LifeCrystalLimiting = true;
        }


        public int LifeCrystalsConsumed { get; internal set; }
        public float HeartContainersPercentage => (5 + LifeCrystalsConsumed) / 20f;

        public static bool LifeCrystalLimiting { get; private set; }
    }
}
