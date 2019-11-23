using Terraria.ID;

namespace WebmilioCommons.Items.Standard
{
    // Credit to Scalie for original implementation/idea
    public class StandardTileItem : StandardItem
    {
        private const int USE_TIME = 10;


        public StandardTileItem(string displayName, string tooltip, int width, int height, int placedTileType, int rarity = ItemRarityID.White, int maxStack = 999, 
            int itemUseStyle = ItemUseStyleID.SwingThrow, int itemUseTime = USE_TIME, int useAnimation = USE_TIME, bool autoReuse = true, bool consumable = true) : 
            base(displayName, tooltip, width, height, rarity: rarity)
        {
            MaxStack = maxStack;

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