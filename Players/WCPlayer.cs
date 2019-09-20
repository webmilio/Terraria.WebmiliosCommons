using System;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace WebmilioCommons.Players
{
    public sealed class WCPlayer : ModPlayer
    {
        public override void OnEnterWorld(Player player)
        {
            /*if (player.whoAmI == Main.myPlayer)
                new PlayerSynchronizationPacket(this).Send();*/
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