using Terraria;
using Terraria.ModLoader;

namespace WebmilioCommons.Buffs
{
    public abstract class StandardBuff : ModBuff
    {
        protected StandardBuff(string displayName, string description, bool hideTime = false, bool save = false, bool persistent = false)
        {
            DefaultDisplayName = displayName;
            DefaultDescription = description;

            HideTime = hideTime;
            Save = save;
            Persistent = persistent;
        }


        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            DisplayName.SetDefault(DefaultDisplayName);

            if (!string.IsNullOrWhiteSpace(DefaultDescription))
                Description.SetDefault(DefaultDescription);

            Main.buffNoTimeDisplay[Type] = HideTime;
            Main.buffNoSave[Type] = !Save;
            Main.persistentBuff[Type] = Persistent;
        }


        protected string DefaultDisplayName { get; }
        protected string DefaultDescription { get; }

        public bool HideTime { get; }
        public bool Save { get; }
        public bool Persistent { get; }
    }
}