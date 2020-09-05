using System.Collections.Generic;
using Terraria.ID;
using Terraria.Localization;

namespace WebmilioCommons.Items.Standard
{
    // Credit to Scalie for original implementation/idea
    public abstract class StandardTileItem : StandardItem
    {
        private const int USE_TIME = 10;


        protected StandardTileItem((GameCulture culture, string displayName, string tooltip)[] strings, int width, int height, int value = 0, int defense = 0, int rarity = ItemRarityID.White, int maxStack = 999) :
            base(strings, width, height, value, defense, rarity, maxStack)
        {
        }

        protected StandardTileItem(string displayName, string tooltip, int width, int height, int placedTileType, int rarity = ItemRarityID.White, int value = 0, int maxStack = 999, 
            int itemUseStyle = ItemUseStyleID.SwingThrow, int itemUseTime = USE_TIME, int useAnimation = USE_TIME, bool autoReuse = true, bool consumable = true) : 
            this(
                new Dictionary<GameCulture, string>()
                {
                    { GameCulture.English, displayName }
                },
                new Dictionary<GameCulture, string>()
                {
                    { GameCulture.English, tooltip }
                }, 
                width, height, placedTileType, rarity, value, maxStack, itemUseStyle, itemUseTime, useAnimation, autoReuse, consumable)
        {
        }

        protected StandardTileItem(Dictionary<GameCulture, string> displayNames, Dictionary<GameCulture, string> tooltips, int width, int height, int placedTileType, int rarity = ItemRarityID.White, int value = 0, int maxStack = 999,
            int itemUseStyle = ItemUseStyleID.SwingThrow, int itemUseTime = USE_TIME, int useAnimation = USE_TIME, bool autoReuse = true, bool consumable = true) :
            base(displayNames, tooltips, width, height, rarity: rarity, value: value, maxStack: maxStack)
        {
            PlacedTileType = placedTileType;

            ItemUseStyle = itemUseStyle;
            ItemUseTime = itemUseTime;
            ItemUseAnimation = useAnimation;

            AutoReuse = autoReuse;
            Consumable = consumable;
        }


        public override void SetDefaults()
        {
            base.SetDefaults();

            item.maxStack = MaxStack;

            item.createTile = PlacedTileType;

            item.useStyle = ItemUseStyle;
            item.useTime = ItemUseTime;
            item.useAnimation = ItemUseAnimation;

            item.autoReuse = AutoReuse;
            item.consumable = Consumable;

            item.useTurn = true;
        }


        public int PlacedTileType { get; }

        public int ItemUseStyle { get; }
        public int ItemUseTime { get; }
        public int ItemUseAnimation { get; }

        public bool AutoReuse { get; }
        public bool Consumable { get; }
    }
}