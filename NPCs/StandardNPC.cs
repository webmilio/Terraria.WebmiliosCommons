using System.Collections.Generic;
using Terraria.Localization;
using Terraria.ModLoader;

namespace WebmilioCommons.NPCs
{
    public abstract class StandardNPC : ModNPC
    {
        protected int lifeMax, defense, value, width, height;


        protected StandardNPC()
        {
        }

        protected StandardNPC((GameCulture culture, string displayName)[] displayNames, int width, int height, int lifeMax, int defense, int value = 0) : 
            this(new Dictionary<GameCulture, string>(), width, height, lifeMax, defense, value)
        {
            foreach (var str in displayNames)
                DisplayNames.Add(str.culture, str.displayName);
        }

        protected StandardNPC(string displayName, int width, int height, int lifeMax, int defense, int value = 0) : this(
            new Dictionary<GameCulture, string>()
            {
                { GameCulture.English, displayName }
            }, 
            width, height, lifeMax, defense, value)
        {
        }

        protected StandardNPC(Dictionary<GameCulture, string> displayNames, int width, int height, int lifeMax, int defense, int value = 0)
        {
            DisplayNames = displayNames;

            this.width = width;
            this.height = height;

            this.lifeMax = lifeMax;
            this.defense = defense;
            this.value = value;
        }


        public override void SetDefaults()
        {
            npc.width = width;
            npc.height = height;

            npc.lifeMax = lifeMax;
            npc.defense = defense;
            npc.value = value;

            base.SetDefaults();
            ModDefaults();
        }

        public virtual void ModDefaults() { }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault(DisplayNames[GameCulture.English]);

            foreach (KeyValuePair<GameCulture, string> displayName in DisplayNames)
                DisplayName.AddTranslation(displayName.Key, displayName.Value);

            base.SetStaticDefaults();
            ModStaticDefaults();
        }

        public virtual void ModStaticDefaults() { }


        protected Dictionary<GameCulture, string> DisplayNames { get; }
    }
}