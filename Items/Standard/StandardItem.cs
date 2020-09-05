using System.Collections.Generic;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace WebmilioCommons.Items.Standard
{
    public abstract class StandardItem : ModItem
    {
        protected readonly int width, height;


        protected StandardItem((GameCulture culture, string displayName, string tooltip)[] strings, int width, int height, int value = 0, int defense = 0, int rarity = ItemRarityID.White, int maxStack = 1) :
            this(new Dictionary<GameCulture, string>(), new Dictionary<GameCulture, string>(), width, height, value, defense, rarity, maxStack)
        {
            foreach (var str in strings)
            {
                DisplayNames.Add(str.culture, str.displayName);
                Tooltips.Add(str.culture, str.tooltip);
            }
        }

        protected StandardItem(string displayName, string tooltip, int width, int height, int value = 0, int defense = 0, int rarity = ItemRarityID.White, int maxStack = 1) : 
            this(
                new Dictionary<GameCulture, string>()
                {
                    { GameCulture.English, displayName }
                },
                new Dictionary<GameCulture, string>()
                {
                    { GameCulture.English, tooltip }
                }, width, height, value, defense, rarity, maxStack)
        {
        }

        protected StandardItem(Dictionary<GameCulture, string> displayNames, Dictionary<GameCulture, string> tooltips, int width, int height, int value = 0, int defense = 0, int rarity = ItemRarityID.White, int maxStack = 1)
        {
            DisplayNames = displayNames;
            Tooltips = tooltips;

            this.width = width;
            this.height = height;

            Value = value;
            Defense = defense;
            Rarity = rarity;

            MaxStack = maxStack;
        }
        

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            DisplayName.SetDefault(DisplayNames[GameCulture.English]);
            Tooltip.SetDefault(Tooltips[GameCulture.English]);

            foreach (KeyValuePair<GameCulture, string> displayName in DisplayNames)
                DisplayName.AddTranslation(displayName.Key, displayName.Value);

            foreach (KeyValuePair<GameCulture, string> tooltip in Tooltips)
                Tooltip.AddTranslation(tooltip.Key, tooltip.Value);

            PostSetStaticDefaults();
        }

        public virtual void PostSetStaticDefaults() { }


        public override void SetDefaults()
        {
            base.SetDefaults();

            item.width = width;
            item.height = height;

            item.value = Value;
            item.defense = Defense;
            item.rare = Rarity;

            item.maxStack = MaxStack;

            PostSetDefaults();
        }

        public virtual void PostSetDefaults() { }


        protected Dictionary<GameCulture, string> DisplayNames { get; }
        protected Dictionary<GameCulture, string> Tooltips { get; }

        public int Value { get; }
        public int Defense { get; }
        public int Rarity { get; }

        public int MaxStack { get; }


        public static class TooltipLines
        {
            public const string
                ITEM_NAME = "ItemName",
                TOOLTIP_LINE_NAME = "Tooltip";

            public static string GetTooltipLineName(int number = 0) => TOOLTIP_LINE_NAME + number;
        }
    }
}