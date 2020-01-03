using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace WebmilioCommons.Buffs
{
    public abstract class StandardBuff : ModBuff
    {
        protected StandardBuff(string displayName, string description, bool hideTime = false, bool save = false, bool persistent = false, bool canBeCleared = true) :
            this(
                new Dictionary<GameCulture, string>()
                {
                    {GameCulture.English, displayName}
                },
                new Dictionary<GameCulture, string>()
                {
                    {GameCulture.English, description}
                },
                hideTime, save, persistent, canBeCleared)
        {
        }

        protected StandardBuff(Dictionary<GameCulture, string> displayNames, Dictionary<GameCulture, string> descriptions, bool hideTime = false, bool save = false, bool persistent = false, bool canBeCleared = true)
        {
            DisplayNames = displayNames;
            Descriptions = descriptions;

            HideTime = hideTime;
            Save = save;
            Persistent = persistent;

            CanBeCleared = canBeCleared;
        }


        public override void SetDefaults()
        {
            base.SetDefaults();

            DisplayName.SetDefault(DisplayNames[GameCulture.English]);
            Description.SetDefault(Descriptions[GameCulture.English]);

            foreach (KeyValuePair<GameCulture, string> displayName in DisplayNames)
                DisplayName.AddTranslation(displayName.Key, displayName.Value);

            foreach (KeyValuePair<GameCulture, string> tooltip in Descriptions)
                Description.AddTranslation(tooltip.Key, tooltip.Value);

            Main.buffNoTimeDisplay[Type] = HideTime;
            Main.buffNoSave[Type] = !Save;
            Main.persistentBuff[Type] = Persistent;

            canBeCleared = CanBeCleared;
        }


        protected Dictionary<GameCulture, string> DisplayNames { get; }
        protected Dictionary<GameCulture, string> Descriptions { get; }

        public bool HideTime { get; }
        public bool Save { get; }
        public bool Persistent { get; }
        public bool CanBeCleared { get; }
    }
}