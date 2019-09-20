using Terraria;
using Terraria.ModLoader;

namespace WebmilioCommons.Extensions
{
    public static class BuffCheckExtensions
    {
        public static bool HasBuff<T>(this Player player) where T : ModBuff => player.HasBuff(typeof(T).GetModFromType().BuffType<T>());
        public static bool HasBuff<T>(this NPC npc) where T : ModBuff => npc.HasBuff(typeof(T).GetModFromType().BuffType<T>());

        public static void AddBuff<T>(this Player player, int time, bool quiet = true) where T : ModBuff => player.AddBuff(typeof(T).GetModFromType().BuffType<T>(), time, quiet);
        public static void AddBuff<T>(this NPC npc, int time, bool quiet = true) where T : ModBuff => npc.AddBuff(typeof(T).GetModFromType().BuffType<T>(), time, quiet);

        public static void ClearBuff<T>(this Player player) where T : ModBuff => player.ClearBuff(typeof(T).GetModFromType().BuffType<T>());
        public static void ClearBuff<T>(this NPC npc) where T : ModBuff => npc.DelBuff(typeof(T).GetModFromType().BuffType<T>());
    }
}