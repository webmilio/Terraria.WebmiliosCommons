using Terraria.ID;

namespace WebmilioCommons.Items.Standard
{
    public abstract class StandardAccessory : StandardItem
    {
        protected StandardAccessory(string displayName, string tooltip, int width, int height, int value = 0, int defense = 0, int rarity = ItemRarityID.White) : 
            base(displayName, tooltip, width, height, value, defense, rarity)
        {
        }


        public override void SetDefaults()
        {
            base.SetDefaults();

            item.accessory = true;
        }
    }
}
