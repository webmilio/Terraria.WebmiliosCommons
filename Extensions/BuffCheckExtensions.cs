using System;
using Terraria;
using Terraria.ModLoader;

namespace WebmilioCommons.Extensions
{
    public static class BuffCheckExtensions
    {
        public static bool HasBuff<T>(this ModPlayer modPlayer) where T : ModBuff => HasBuff<T>(modPlayer.Player);
        public static bool HasBuff<T>(this Player player) where T : ModBuff => player.HasBuff(ModContent.BuffType<T>());
        public static bool HasBuff<T>(this NPC npc) where T : ModBuff => npc.HasBuff(ModContent.BuffType<T>());


        [Obsolete("Work-in-progress")]
        public static bool HasBuffSubclass<T>(this ModPlayer modPlayer) where T : ModBuff => HasBuffSubclass<T>(modPlayer.Player);

        [Obsolete("Work-in-progress")]
        public static bool HasBuffSubclass<T>(this Player player) where T : ModBuff
        {
            for (int i = 0; i < player.buffTime.Length; i++)
            {
                if (player.buffTime[i] <= 0)
                    continue;

                ModBuff buff = BuffLoader.GetBuff(i);

                if (buff == default)
                    continue;

                if (buff is T)
                    return true;
            }

            return false;
        }


        public static void AddBuff<T>(this ModPlayer modPlayer, int time, bool quiet = true) where T : ModBuff => AddBuff<T>(modPlayer.Player, time, quiet);
        public static void AddBuff<T>(this Player player, int time, bool quiet = true) where T : ModBuff => player.AddBuff(ModContent.BuffType<T>(), time, quiet);
        public static void AddBuff<T>(this NPC npc, int time, bool quiet = true) where T : ModBuff => npc.AddBuff(ModContent.BuffType<T>(), time, quiet);

        public static void ClearBuff<T>(this ModPlayer modPlayer) where T : ModBuff => ClearBuff<T>(modPlayer.Player);
        public static void ClearBuff<T>(this Player player) where T : ModBuff => player.ClearBuff(ModContent.BuffType<T>());
        public static void ClearBuff<T>(this NPC npc) where T : ModBuff => npc.DelBuff(ModContent.BuffType<T>());
    }
}