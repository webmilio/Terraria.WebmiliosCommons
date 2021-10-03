using System.Collections.Generic;
using System.Linq;
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
                    {WebmilioCommonsMod.EnglishCulture, displayName}
                },
                new Dictionary<GameCulture, string>()
                {
                    {WebmilioCommonsMod.EnglishCulture, description}
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

            this.Register();
        }

        protected StandardBuff((GameCulture culture, string displayName, string description) str, bool hideTime = false, bool save = false, bool persistent = false, bool canBeCleared = true)
            : this(new[] { str }, hideTime, save, persistent, canBeCleared)
        {
        }

        protected StandardBuff((GameCulture culture, string displayName, string description)[] strings, bool hideTime = false, bool save = false, bool persistent = false, bool canBeCleared = true)
            : this(new Dictionary<GameCulture, string>(), new Dictionary<GameCulture, string>(), hideTime, save, persistent, canBeCleared)
        {
            foreach (var str in strings)
            {
                DisplayNames.Add(str.culture, str.displayName);
                Descriptions.Add(str.culture, str.description);
            }
        }


        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            DisplayName.SetDefault(DisplayNames.First().Value);
            Description.SetDefault(Descriptions.First().Value);

            foreach (KeyValuePair<GameCulture, string> displayName in DisplayNames)
                DisplayName.AddTranslation(displayName.Key, displayName.Value);

            foreach (KeyValuePair<GameCulture, string> tooltip in Descriptions)
                Description.AddTranslation(tooltip.Key, tooltip.Value);

            Main.buffNoTimeDisplay[Type] = HideTime;
            Main.buffNoSave[Type] = !Save;
            Main.persistentBuff[Type] = Persistent;
        }


        protected Dictionary<GameCulture, string> DisplayNames { get; }
        protected Dictionary<GameCulture, string> Descriptions { get; }

        public bool HideTime { get; }
        public bool Save { get; }
        public bool Persistent { get; }
    }
}