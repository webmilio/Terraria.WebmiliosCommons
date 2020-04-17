using Terraria;
using Terraria.ModLoader;

namespace WebmilioCommons.Extensions
{
    public static class ChestExtensions
    {
        /// <summary>Adds an item to a shop. Use in <see cref="GlobalNPC.SetupShop"/>.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="shop"></param>
        /// <param name="nextSlot"></param>
        public static void AddShop<T>(this Chest shop, ref int nextSlot) where T : ModItem => AddShop<T>(shop, 1, ref nextSlot);

        /// <summary>Adds an item to a shop. Use in <see cref="GlobalNPC.SetupShop"/>.</summary>
        /// <typeparam name="T">The item type of the ModItem.</typeparam>
        /// <param name="shop"></param>
        /// <param name="stack">How many items in the stack being sold.</param>
        /// <param name="nextSlot"></param>
        public static void AddShop<T>(this Chest shop, int stack, ref int nextSlot) where T : ModItem => AddShop(shop, ModContent.ItemType<T>(), stack, ref nextSlot);


        /// <summary>Adds an item to a shop. Use in <see cref="GlobalNPC.SetupShop"/>.</summary>
        /// <param name="shop"></param>
        /// <param name="type">The item type of the ModItem.</param>
        /// <param name="nextSlot"></param>
        public static void AddShop(this Chest shop, int type, ref int nextSlot) => AddShop(shop, type, 1, ref nextSlot);

        /// <summary>Adds an item to a shop. Use in <see cref="GlobalNPC.SetupShop"/>.</summary>
        /// <param name="shop"></param>
        /// <param name="type"></param>
        /// <param name="stack">How many items in the stack being sold.</param>
        /// <param name="nextSlot"></param>
        public static void AddShop(this Chest shop, int type, int stack, ref int nextSlot)
        {
            Item item = new Item();
            item.SetDefaults(type);
            item.stack = stack;

            shop.AddShop(item);
            nextSlot++;
        }
    }
}