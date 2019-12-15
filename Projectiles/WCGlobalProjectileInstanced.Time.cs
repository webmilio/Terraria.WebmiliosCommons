using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using WebmilioCommons.States;
using WebmilioCommons.Time;

namespace WebmilioCommons.Projectiles
{
    public sealed partial class WCGlobalProjectileInstanced : GlobalProjectile
    {
        private bool PreAITime(Projectile projectile)
        {
            if (CurrentRequest != TimeManagement.CurrentRequest)
                CurrentRequest = !TimeManagement.TimeAltered ? null : TimeManagement.CurrentRequest;

            if (CurrentRequest == null || !CurrentRequest.AlterNPCs || TimeManagement.IsProjectileImmune(projectile))
            {
                if (State != null)
                {
                    State.Restore();
                    State = null;
                }

                TimeAltered = false;
                return true;
            }

            TimeAltered = true;

            if (TimeAltered && State == null)
                State = new ProjectileInstantState(projectile);

            CanRunCurrentTick = CurrentRequest.TickRate != 0 && TimeManagement.CurrentTick % CurrentRequest.TickRate == 0;

            if (CanRunCurrentTick)
            {
                State.Restore();
                State = null;

                return true;
            }

            State.PreAI(projectile);
            projectile.frameCounter = 0;
            return false;
        }


        public TimeAlterationRequest CurrentRequest { get; private set; }

        public bool TimeAltered { get; private set; }
        public bool CanRunCurrentTick { get; private set; }

        public ProjectileInstantState State { get; private set; }
    }
}
