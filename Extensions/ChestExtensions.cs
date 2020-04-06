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
        public static void AddShop<T>(this Chest shop, ref int nextSlot) where T : ModItem
        {
            Item item = new Item();
            item.SetDefaults(ModContent.ItemType<T>());

            shop.AddShop(item);
            nextSlot++;
        }
    }
}