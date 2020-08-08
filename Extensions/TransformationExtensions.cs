using Terraria;
using Terraria.ModLoader;
using WebmilioCommons.Players;
using WebmilioCommons.Transformations;

namespace WebmilioCommons.Extensions
{
    public static class TransformationExtensions
    {
        public static bool Transform(this Player player, PlayerTransformation transformation) => WCPlayer.Get(player).Transform(transformation);
        public static bool Transform(this ModPlayer player, PlayerTransformation transformation) => WCPlayer.Get(player).Transform(transformation);

        public static bool Transform<T>(this Player player) where T : PlayerTransformation => WCPlayer.Get(player).Transform<T>();
        public static bool Transform<T>(this ModPlayer player) where T : PlayerTransformation => WCPlayer.Get(player).Transform<T>();


        public static bool IsTransformed(this Player player, PlayerTransformation transformation) => WCPlayer.Get(player).IsTransformed(transformation);
        public static bool IsTransformed(this ModPlayer player, PlayerTransformation transformation) => WCPlayer.Get(player).IsTransformed(transformation);

        public static bool IsTransformed<T>(this Player player, PlayerTransformation transformation) where T : PlayerTransformation => WCPlayer.Get(player).IsTransformed(transformation);
        public static bool IsTransformed<T>(this ModPlayer player, PlayerTransformation transformation) where T : PlayerTransformation => WCPlayer.Get(player).IsTransformed(transformation);


        public static void DeTransform(this Player player) => WCPlayer.Get(player).DeTransform();
        public static void DeTransform(this ModPlayer player) => WCPlayer.Get(player).DeTransform();

        public static bool DeTransform<T>(this Player player) where T : PlayerTransformation => WCPlayer.Get(player).DeTransform<T>();
        public static bool DeTransform<T>(this ModPlayer player) where T : PlayerTransformation => WCPlayer.Get(player).DeTransform<T>();

        public static bool DeTransform(this Player player, PlayerTransformation transformation) => WCPlayer.Get(player).DeTransform(transformation);
        public static bool DeTransform(this ModPlayer player, PlayerTransformation transformation) => WCPlayer.Get(player).DeTransform(transformation);
    }
}