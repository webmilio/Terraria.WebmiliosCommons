using System.Collections.Generic;
using Terraria.ID;
using Terraria.Localization;

namespace WebmilioCommons.Items.Standard
{
    public abstract class StandardAccessory : StandardItem
    {
        protected StandardAccessory((GameCulture culture, string displayName, string tooltip) str, int width, int height, int value = 0, int defense = 0, int rarity = ItemRarityID.White, int maxStack = 1) :
            base(str, width, height, value, defense, rarity, maxStack)
        {
        }

        protected StandardAccessory((GameCulture culture, string displayName, string tooltip)[] strings, int width, int height, int value = 0, int defense = 0, int rarity = ItemRarityID.White, int maxStack = 1) :
            base(strings, width, height, value, defense, rarity, maxStack)
        {
        }

        protected StandardAccessory(string displayName, string tooltip, int width, int height, int value = 0, int defense = 0, int rarity = ItemRarityID.White, int maxStack = 1) : 
            base(displayName, tooltip, width, height, value, defense, rarity, maxStack)
        {
        }

        protected StandardAccessory(Dictionary<GameCulture, string> displayNames, Dictionary<GameCulture, string> tooltips, int width, int height, int value = 0, int defense = 0, int rarity = ItemRarityID.White, int maxStack = 1) :
            base(displayNames, tooltips, width, height, value, defense, rarity, maxStack)
        {
        }


        public override void SetDefaults()
        {
            base.SetDefaults();

            item.accessory = true;
        }
    }
}
