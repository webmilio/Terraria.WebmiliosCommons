using System;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using WebmilioCommons.Extensions;

namespace WebmilioCommons.Players
{
    public sealed class WCPlayer : ModPlayer
    {
        public WCPlayer Get() => Get(Main.LocalPlayer);
        public WCPlayer Get(Player player) => player.GetModPlayer<WCPlayer>();
        public WCPlayer Get(ModPlayer modPlayer) => Get(modPlayer.player);


        public override void OnEnterWorld(Player player)
        {
            this.SendIfLocal<WCPlayerOnJoinWorld>();
        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            /*KeyStates keyState = KeyboardManager.GetKeyState(Keys.U);

            if (keyState != KeyStates.NotPressed)
                Main.NewText($"Key U State: {keyState}");*/
        }


        public override TagCompound Save()
        {
            return new TagCompound
            {
                { nameof(UniqueID), UniqueID.ToString() }
            };
        }

        public override void Load(TagCompound tag)
        {
            UniqueID = tag.ContainsKey(nameof(UniqueID)) ? Guid.Parse(tag.GetString(nameof(UniqueID))) : Guid.NewGuid();
        }


        public Guid UniqueID { get; internal set; }
    }
}