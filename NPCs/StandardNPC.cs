using System.Collections.Generic;
using Terraria.Localization;
using Terraria.ModLoader;

namespace WebmilioCommons.NPCs
{
    public abstract class StandardNPC : ModNPC
    {
        protected int life, defense, value;


        protected StandardNPC(string displayName, int life, int defense, int value = 0) : this(
            new Dictionary<GameCulture, string>()
            {
                { GameCulture.English, displayName }
            }, 
            life, defense, value)
        {
        }

        protected StandardNPC(Dictionary<GameCulture, string> displayNames, int life, int defense, int value = 0)
        {
            DisplayNames = displayNames;

            this.life = life;
            this.defense = defense;
            this.value = value;
        }


        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault(DisplayNames[GameCulture.English]);

            foreach (KeyValuePair<GameCulture, string> displayName in DisplayNames)
                DisplayNames.Add(displayName.Key, displayName.Value);

            base.SetStaticDefaults();
        }


        protected Dictionary<GameCulture, string> DisplayNames { get; }
    }
}