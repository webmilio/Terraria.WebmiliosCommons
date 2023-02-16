using Terraria;
using Terraria.ModLoader;

namespace WebCom.Extensions
{
    public static class ChestExtensions
    {
        /// <summary>Adds an item to a shop. Use in <see cref="GlobalNPC.SetupShop"/>.</summary>
        /// <typeparam name="T"></typeparam>
        public static void AddItemToShop<T>(this Chest shop, ref int nextSlot) where T : ModItem => AddItemToShop<T>(shop, 1, ref nextSlot);

        /// <summary>Adds an item to a shop. Use in <see cref="GlobalNPC.SetupShop"/>.</summary>
        /// <typeparam name="T">The item type of the ModItem.</typeparam>
        /// <param name="stack">How many items in the stack being sold.</param>
        public static void AddItemToShop<T>(this Chest shop, int stack, ref int nextSlot) where T : ModItem => AddItemToShop(shop, ModContent.ItemType<T>(), stack, ref nextSlot);


        /// <summary>Adds an item to a shop. Use in <see cref="GlobalNPC.SetupShop"/>.</summary>
        /// <param name="type">The item type of the ModItem.</param>
        public static void AddItemToShop(this Chest shop, int type, ref int nextSlot) => AddItemToShop(shop, type, 1, ref nextSlot);

        /// <summary>Adds an item to a shop. Use in <see cref="GlobalNPC.SetupShop"/>.</summary>
        /// <param name="stack">How many items in the stack being sold.</param>
        public static void AddItemToShop(this Chest shop, int type, int stack, ref int nextSlot)
        {
            Item item = new Item();
            item.SetDefaults(type);
            item.stack = stack;

            shop.AddItemToShop(item);
            nextSlot++;
        }
    }
}