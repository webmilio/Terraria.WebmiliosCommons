using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using WebmilioCommons.Animations;
using WebmilioCommons.Extensions;

namespace WebmilioCommons.Players
{
    public sealed partial class WCPlayer : ModPlayer
    {
        public static WCPlayer Get() => Get(Main.LocalPlayer);
        public static WCPlayer Get(Player player) => player.GetModPlayer<WCPlayer>();
        public static WCPlayer Get(ModPlayer modPlayer) => Get(modPlayer.player);


        private List<PlayerAnimation> _currentAnimations;


        #region Animations

        public bool BeginAnimation(PlayerAnimation animation)
        {
            if (_currentAnimations.Find(a => a.UnlocalizedName == animation.UnlocalizedName && a.Unique) == null)
                return false;

            _currentAnimations.Add(animation);
            animation.Begin();

            return true;
        }

        public bool EndAnimation(PlayerAnimation animation)
        {
            if (!_currentAnimations.Contains(animation))
                return false;

            animation.End();
            _currentAnimations.Remove(animation);

            return true;
        }

        public void EndAllAnimations() => ForAllAnimations(animation => EndAnimation(animation));


        public bool HasAnimation(PlayerAnimation animation) => _currentAnimations.Contains(animation);

        public void ForAllAnimations(Action<PlayerAnimation> action)
        {
            for (int i = 0; i < _currentAnimations.Count; i++)
                action(_currentAnimations[i]);
        }

        #endregion


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
            _currentAnimations = new List<PlayerAnimation>();
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

        #endregion



        public Guid UniqueID { get; internal set; }
    }
}