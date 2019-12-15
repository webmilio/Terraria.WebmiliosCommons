using Terraria.ModLoader;

namespace WebmilioCommons.NPCs
{
    public abstract class StandardNPC : ModNPC
    {
        protected string displayName;
        protected int life, defense, value;


        protected StandardNPC(string displayName, int life, int defense, int value = 0)
        {
            this.displayName = displayName;

            this.life = life;
            this.defense = defense;
            this.value = value;
        }


        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault(displayName);

            base.SetStaticDefaults();
        }
    }
}