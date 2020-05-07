using System.Collections.Generic;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using WebmilioCommons.Items.Standard;

namespace WebmilioCommons.Statues
{
    public abstract class QuickStatueTileItem : StandardTileItem
    {
        protected QuickStatueTileItem(string displayName, string tooltip, int width, int height, int placedTileType, int rarity = ItemRarityID.White, int value = 0, int maxStack = 999, int itemUseStyle = ItemUseStyleID.SwingThrow, int itemUseTime = 10, int useAnimation = 10, bool autoReuse = true, bool consumable = true) : base(displayName, tooltip, width, height, placedTileType, rarity, value, maxStack, itemUseStyle, itemUseTime, useAnimation, autoReuse, consumable)
        {
        }

        protected QuickStatueTileItem(Dictionary<GameCulture, string> displayNames, Dictionary<GameCulture, string> tooltips, int width, int height, int placedTileType, int rarity = ItemRarityID.White, int value = 0, int maxStack = 999, int itemUseStyle = ItemUseStyleID.SwingThrow, int itemUseTime = 10, int useAnimation = 10, bool autoReuse = true, bool consumable = true) : base(displayNames, tooltips, width, height, placedTileType, rarity, value, maxStack, itemUseStyle, itemUseTime, useAnimation, autoReuse, consumable)
        {
        }


        public override void SetDefaults()
        {
            base.SetDefaults();

            item.CloneDefaults(ItemID.ArmorStatue);

            item.createTile = PlacedTileType;
            item.placeStyle = 0;
        }
    }
}