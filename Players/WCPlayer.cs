using System;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using WebmilioCommons.Extensions;

namespace WebmilioCommons.Players
{
    public sealed partial class WCPlayer : ModPlayer
    {
        public static WCPlayer Get() => Get(Main.LocalPlayer);
        public static WCPlayer Get(Player player) => player.GetModPlayer<WCPlayer>();
        public static WCPlayer Get(ModPlayer modPlayer) => Get(modPlayer.player);


        #region Hooks

        #region Save/Load

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

        #endregion

        public override void Initialize()
        {
            InitializeAnimations();
        }

        public override void OnEnterWorld(Player player)
        {
            this.SendIfLocal<WCPlayerOnJoinWorld>();
        }


        public override void PreUpdate() => ForAllAnimations(animation => animation.HandlePreUpdate());
        public override void PreUpdateBuffs() => ForAllAnimations(animation => animation.HandlePreUpdateBuffs());
        public override void PreUpdateMovement() => ForAllAnimations(animation => animation.HandlePreUpdateMovements());

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            /*KeyStates keyState = KeyboardManager.GetKeyState(Keys.U);

            if (keyState != KeyStates.NotPressed)
                Main.NewText($"Key U State: {keyState}");*/
        }

        public override void UpdateEquips(ref bool wallSpeedBuff, ref bool tileSpeedBuff, ref bool tileRangeBuff)
        {
            bool 
                wallSpeedBuffs2 = wallSpeedBuff,
                tileSpeedBuffs2 = tileSpeedBuff,
                tileRangeBuff2 = tileRangeBuff;

            ForAllAnimations(animation => animation.HandleUpdateEquips(wallSpeedBuffs2, tileSpeedBuffs2, tileRangeBuff2));
        }

        public override void PostUpdate() => ForAllAnimations(animation => animation.HandlePostUpdate());

        #endregion



        public Guid UniqueID { get; internal set; }
    }
}