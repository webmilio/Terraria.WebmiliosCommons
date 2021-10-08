using System;
using System.Collections.Generic;
using Terraria.ID;
using Terraria.Localization;

namespace WebmilioCommons.Items.Standard
{
    // Credit to Scalie for original implementation/idea
    public abstract class StandardTileItem : StandardItem
    {
        [Obsolete("Use " + nameof(UseTime) + ".", true)]
        public const int USE_TIME = UseTime;
        public const int UseTime = 10;

        protected StandardTileItem(string displayName, string tooltip, int width, int height, int placedTileType, int rarity = ItemRarityID.White, int value = 0, int maxStack = 999, 
            int itemUseStyle = ItemUseStyleID.Swing, int itemUseTime = UseTime, int useAnimation = UseTime, bool autoReuse = true, bool consumable = true) : 
            this(
                new Dictionary<GameCulture, string>()
                {
                    { WebmilioCommonsMod.EnglishCulture, displayName }
                },
                new Dictionary<GameCulture, string>()
                {
                    { WebmilioCommonsMod.EnglishCulture, tooltip }
                }, 
                width, height, placedTileType, rarity, value, maxStack, itemUseStyle, itemUseTime, useAnimation, autoReuse, consumable)
        {
        }

        protected StandardTileItem(Dictionary<GameCulture, string> displayNames, Dictionary<GameCulture, string> tooltips, int width, int height, int placedTileType, int rarity = ItemRarityID.White, int value = 0, int maxStack = 999,
            int itemUseStyle = ItemUseStyleID.Swing, int itemUseTime = UseTime, int useAnimation = UseTime, bool autoReuse = true, bool consumable = true) :
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

            Item.maxStack = MaxStack;

            Item.createTile = PlacedTileType;

            Item.useStyle = ItemUseStyle;
            Item.useTime = ItemUseTime;
            Item.useAnimation = ItemUseAnimation;

            Item.autoReuse = AutoReuse;
            Item.consumable = Consumable;

            Item.useTurn = true;
        }


        public int PlacedTileType { get; }

        public int ItemUseStyle { get; }
        public int ItemUseTime { get; }
        public int ItemUseAnimation { get; }

        public bool AutoReuse { get; }
        public bool Consumable { get; }
    }
}