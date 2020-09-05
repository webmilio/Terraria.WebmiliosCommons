using System.Collections.Generic;
using Terraria.ID;
using Terraria.Localization;

namespace WebmilioCommons.Items.Standard
{
    public abstract class StandardAccessory : StandardItem
    {
        protected StandardAccessory(string displayName, string tooltip, int width, int height, int value = 0, int defense = 0, int rarity = ItemRarityID.White, int maxStack = 1) : 
            this(
                new Dictionary<GameCulture, string>()
                {
                    { GameCulture.English, displayName }
                },
                new Dictionary<GameCulture, string>()
                {
                    { GameCulture.English, tooltip }
                }, 
                width, height, value, defense, rarity, maxStack)
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
