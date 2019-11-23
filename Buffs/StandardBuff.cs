using Terraria;
using Terraria.ModLoader;

namespace WebmilioCommons.Buffs
{
    public abstract class StandardBuff : ModBuff
    {
        protected StandardBuff(string displayName, string tooltip, bool hideTime = false, bool save = false, bool persistent = false, bool canBeCleared = true)
        {
            this.displayName = displayName;
            this.tooltip = tooltip;

            this.hideTime = hideTime;
            this.save = save;
            this.persistent = persistent;

            CanBeCleared = canBeCleared;
        }


        public override void SetDefaults()
        {
            base.SetDefaults();

            DisplayName.SetDefault(displayName);
            Description.SetDefault(tooltip);

            Main.buffNoTimeDisplay[Type] = hideTime;
            Main.buffNoSave[Type] = !save;
            Main.persistentBuff[Type] = persistent;

            canBeCleared = CanBeCleared;
        }


        // ReSharper disable InconsistentNaming
        public string displayName { get; }
        public string tooltip { get; }

        public bool hideTime { get; }
        public bool save { get; }
        public bool persistent { get; }
        public bool CanBeCleared { get; }
        // ReSharper restore InconsistentNaming
    }
}