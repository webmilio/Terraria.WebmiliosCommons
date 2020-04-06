using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace WebmilioCommons.Extensions
{
    public static class ItemExtensions
    {
        public static void SetDefaults(this Item item, ModItem modItem)
        {
            Type itemType = modItem.GetType();

            item.SetDefaults(itemType.GetModFromType().ItemType(itemType.Name));
        }


        public static bool Consume(this Item item, int count = 1)
        {
            if (item.stack < count)
                return false;

            if (item.stack > 1)
                item.stack--;
            else
                item.TurnToAir();

            return true;
        }

        public static bool Consume(this ModItem modItem, int count = 1) => Consume(modItem.item, count);


        public static void Synchronize(this ModItem modItem) => Synchronize(modItem.item);

        public static void Synchronize(this Item item) => NetMessage.SendData(MessageID.SyncItem, number: item.whoAmI);


        #region Item Checks

        public const int
            ARMOR_SLOTS_COUNT = 3,
            ACCESSORY_SLOTS_COUNT = 5,
            MAX_EXTRA_ACCESSORY_SLOTS = 2,

            SOCIAL_ARMOR_START_INDEX = ARMOR_SLOTS_COUNT + ACCESSORY_SLOTS_COUNT + MAX_EXTRA_ACCESSORY_SLOTS;

        public static List<T> GetItemsByType<T>(this Player player, bool inventory = false, bool armor = false, bool armorSocial = false, bool accessories = false, bool accessoriesSocial = false, bool unique = false, Predicate<Item> condition = null) where T : class
        {
            List<T> filteredItems = new List<T>();

            if (inventory)
                SearchItems(ref filteredItems, player.inventory, unique, condition);

            if (armor) // TODO Verify this works.
            {
                Item[] armorSlots = new Item[ARMOR_SLOTS_COUNT];
                Array.Copy(player.armor, 0, armorSlots, 0, armorSlots.Length);

                SearchItems(ref filteredItems, armorSlots, unique, condition);
            }

            if (accessories)
            {
                Item[] accessorySlots = new Item[ACCESSORY_SLOTS_COUNT + player.extraAccessorySlots];
                Array.Copy(player.armor, ARMOR_SLOTS_COUNT, accessorySlots, 0, accessorySlots.Length);

                SearchItems(ref filteredItems, accessorySlots, unique, condition);
            }

            if (armorSocial) // TODO Verify this works.
            {
                Item[] armorSlots = new Item[ARMOR_SLOTS_COUNT];
                Array.Copy(player.armor, SOCIAL_ARMOR_START_INDEX, armorSlots, 0, armorSlots.Length);

                SearchItems(ref filteredItems, armorSlots, unique, condition);
            }

            if (accessoriesSocial)
            {
                Item[] accessorySlots = new Item[ACCESSORY_SLOTS_COUNT + player.extraAccessorySlots];
                Array.Copy(player.armor, SOCIAL_ARMOR_START_INDEX + ARMOR_SLOTS_COUNT, accessorySlots, 0, accessorySlots.Length);

                SearchItems(ref filteredItems, accessorySlots, unique, condition);
            }

            return filteredItems;
        }

        public static List<Item> GetItemsByType(this Player player, int type, bool inventory = false, bool armor = false, bool armorSocial = false, bool accessories = false, bool accessoriesSocial = false, bool unique = false, Predicate<Item> condition = null)
        {
            List<Item> filteredItems = new List<Item>();

            if (inventory)
                SearchItems(ref filteredItems, player.inventory, type, unique, condition);

            if (armor) // TODO Verify this works.
            {
                Item[] armorSlots = new Item[ARMOR_SLOTS_COUNT];
                Array.Copy(player.armor, 0, armorSlots, 0, armorSlots.Length);

                SearchItems(ref filteredItems, armorSlots, type, unique, condition);
            }

            if (accessories)
            {
                Item[] accessorySlots = new Item[ACCESSORY_SLOTS_COUNT + player.extraAccessorySlots];
                Array.Copy(player.armor, ARMOR_SLOTS_COUNT, accessorySlots, 0, accessorySlots.Length);

                SearchItems(ref filteredItems, accessorySlots, type, unique, condition);
            }

            if (armorSocial) // TODO Verify this works.
            {
                Item[] armorSlots = new Item[ARMOR_SLOTS_COUNT];
                Array.Copy(player.armor, SOCIAL_ARMOR_START_INDEX, armorSlots, 0, armorSlots.Length);

                SearchItems(ref filteredItems, armorSlots, type, unique, condition);
            }

            if (accessoriesSocial)
            {
                Item[] accessorySlots = new Item[ACCESSORY_SLOTS_COUNT + player.extraAccessorySlots];
                Array.Copy(player.armor, SOCIAL_ARMOR_START_INDEX + ARMOR_SLOTS_COUNT, accessorySlots, 0, accessorySlots.Length);

                SearchItems(ref filteredItems, accessorySlots, type, unique, condition);
            }

            return filteredItems;
        }


        private static void SearchItems<T>(ref List<T> filtered, IEnumerable<Item> items, bool unique = false, Predicate<Item> condition = default) where T : class
        {
            List<int> foundTypes = new List<int>();

            foreach (Item item in items)
            {
                if (unique && foundTypes.Contains(item.type))
                    continue;

                if (item?.modItem != null && item.IsOfType<T>() && (condition == default || condition(item)))
                {
                    filtered.Add(item.modItem as T);
                    foundTypes.Add(item.type);
                }
            }
        }

        private static void SearchItems(ref List<Item> filtered, IEnumerable<Item> items, int type, bool unique = false, Predicate<Item> condition = default)
        {
            List<int> foundTypes = new List<int>();

            foreach (Item item in items)
            {
                if (unique && foundTypes.Contains(item.type))
                    continue;

                if (item != null && item.type == type && (condition == default || condition(item)))
                {
                    filtered.Add(item);
                    foundTypes.Add(item.type);
                }
            }
        }


        public static Item Find<T>(this Item[] items, int startIndex = 0) where T : ModItem => Find<T>(items, default, startIndex);

        public static Item Find<T>(this Item[] items, Predicate<Item> predicate, int startIndex = 0) where T : ModItem
        {
            int index = FindIndex<T>(items, predicate, startIndex);

            return index == -1 ? default : items[index];
        }


        public static int FindIndex<T>(this Item[] items, int startIndex = 0) where T : ModItem => FindIndex<T>(items, default, startIndex);

        public static int FindIndex<T>(this Item[] items, Predicate<Item> predicate, int startIndex = 0) where T : ModItem
        {
            for (int i = startIndex; i < items.Length; i++)
            {
                Item item = items[i];

                if (item.IsOfType<T>() && (predicate == default || predicate(item)))
                    return i;
            }

            return default;
        }


        public static bool IsOfType<T>(this Item item) where T : class => item.modItem is T;

        #endregion
    }
}