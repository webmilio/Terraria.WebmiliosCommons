using System.Collections.Generic;
using Terraria.ID;
using Terraria.Localization;

namespace WebmilioCommons.Items.Standard
{
    // Credit to Scalie for original implementation/idea
    public abstract class StandardTileItem : StandardItem
    {
        private const int USE_TIME = 10;


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
            PlacedPlacedTileType = placedTileType;

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

            item.type = PlacedPlacedTileType;

            item.useStyle = ItemUseStyle;
            item.useTime = ItemUseTime;
            item.useAnimation = ItemUseAnimation;

            item.useTurn = true;
        }


        public int MaxStack { get; }

        public int PlacedPlacedTileType { get; }

        public int ItemUseStyle { get; }
        public int ItemUseTime { get; }
        public int ItemUseAnimation { get; }

        public bool AutoReuse { get; }
        public bool Consumable { get; }
    }
}