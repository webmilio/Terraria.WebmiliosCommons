using Terraria;
using Terraria.ModLoader;
using WebmilioCommons.Animations;
using WebmilioCommons.Players;

namespace WebmilioCommons.Extensions
{
    public static class AnimationExtensions
    {
        public static bool Begin(this ModPlayer modPlayer, PlayerAnimation animation) => Begin(modPlayer.player, animation);

        public static bool Begin(this Player player, PlayerAnimation animation) => WCPlayer.Get(player).BeginAnimation(animation);


        public static bool End(this ModPlayer modPlayer, PlayerAnimation animation) => End(modPlayer.player, animation);
        public static bool End(this Player player, PlayerAnimation animation) => WCPlayer.Get(player).EndAnimation(animation);
    }
}