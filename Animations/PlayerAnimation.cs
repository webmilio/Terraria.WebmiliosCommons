using System;
using Terraria;
using Terraria.ModLoader;
using WebmilioCommons.Managers;

namespace WebmilioCommons.Animations
{
    public abstract class PlayerAnimation : IHasUnlocalizedName
    {
        public delegate void UpdateMethod();


        protected PlayerAnimation(string unlocalizedName, ModPlayer modPlayer, bool unique = false, int preUpdateTickRate = 0, int preUpdateBuffsTickRate = 0, int preUpdateMovementTickRate = 0, int updateEquipsTickRate = 0, int postUpdateTickRate = 0)
        {
            UnlocalizedName = unlocalizedName;

            Player = modPlayer.player;
            ModPlayer = modPlayer;

            Unique = unique;

            PreUpdateTickRate = preUpdateTickRate;
            PreUpdateBuffsTickRate = preUpdateBuffsTickRate;
            PreUpdateMovementTickRate = preUpdateMovementTickRate;

            UpdateEquipsTickRate = updateEquipsTickRate;

            PostUpdateTickRate = postUpdateTickRate;
        }


        public virtual void Begin() { }

        public virtual void End() { }


        #region PreUpdate

        internal void HandlePreUpdate()
        {
            CurrentPreUpdateFrame++;

            if (CurrentPreUpdateFrame % PreUpdateTickRate == 0)
            {
                CurrentPreUpdateTicks++;
                PreUpdateTick();
            }
        }

        public virtual void PreUpdateTick() { }

        public int PreUpdateTickRate { get; }
        public int CurrentPreUpdateFrame { get; private set; }
        public int CurrentPreUpdateTicks { get; private set; }

        #endregion

        #region PreUpdateBuffs

        internal void HandlePreUpdateBuffs()
        {
            CurrentPreUpdateBuffsFrames++;

            if (CurrentPreUpdateBuffsFrames % PreUpdateBuffsTickRate == 0)
            {
                CurrentPreUpdateBuffsTicks++;
                PreUpdateBuffsTick();
            }
        }

        public virtual void PreUpdateBuffsTick() { }

        public int PreUpdateBuffsTickRate { get; }
        public int CurrentPreUpdateBuffsFrames { get; private set; }
        public int CurrentPreUpdateBuffsTicks { get; private set; }

        #endregion

        #region PreUpdateMovement

        internal void HandlePreUpdateMovements()
        {
            CurrentPreUpdateMovementFrames++;

            if (CurrentPreUpdateMovementFrames % PreUpdateMovementTickRate == 0)
            {
                CurrentPreUpdateMovementTicks++;
                PreUpdateMovementTick();
            }
        }

        public virtual void PreUpdateMovementTick() { }

        public int PreUpdateMovementTickRate { get; }
        public int CurrentPreUpdateMovementFrames { get; private set; }
        public int CurrentPreUpdateMovementTicks { get; private set; }

        #endregion


        #region UpdateEquips

        internal void HandleUpdateEquips(bool wallSpeedBuff, bool tileSpeedBuff, bool tileRangeBuff)
        {
            CurrentUpdateEquipsFrames++;

            if (CurrentUpdateEquipsFrames % UpdateEquipsTickRate == 0)
            {
                CurrentUpdateEquipsTicks++;
                UpdateEquips(wallSpeedBuff, tileSpeedBuff, tileRangeBuff);
            }
        }

        public virtual void UpdateEquips(bool wallSpeedBuff, bool tileSpeedBuff, bool tileRangeBuff) { }

        public int UpdateEquipsTickRate { get; }
        public int CurrentUpdateEquipsFrames { get; private set; }
        public int CurrentUpdateEquipsTicks { get; private set; }

        #endregion

        #region PostUpdate

        internal void HandlePostUpdate()
        {
            CurrentPostUpdateFrames++;

            if (CurrentPostUpdateFrames % PostUpdateTickRate == 0)
            {
                CurrentPostUpdatesTicks++;
                PostUpdate();
            }
        }

        public virtual void PostUpdate() { }

        public int PostUpdateTickRate { get; }
        public int CurrentPostUpdateFrames { get; private set; }
        public int CurrentPostUpdatesTicks { get; private set; }

        #endregion


        public string UnlocalizedName { get; }

        public Player Player { get; }
        public ModPlayer ModPlayer { get; }

        public bool Unique { get; }
    }
}