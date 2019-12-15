using WebmilioCommons.States;
using WebmilioCommons.Time;

namespace WebmilioCommons.Players
{
    public sealed partial class WCPlayer
    {
        private bool PreUpdateTime()
        {
            if (TimeManagement.TimeAltered)
            {

            }

            return true;
        }


        public PlayerInstantState TimeState { get; }
    }
}
