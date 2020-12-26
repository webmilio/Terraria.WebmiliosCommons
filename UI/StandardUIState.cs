using Terraria.UI;

namespace WebmilioCommons.UI
{
    public abstract class StandardUIState : UIState, IHasVisibility
    {
        protected StandardUIState()
        {
        }


        public bool Visible { get; set; }
    }
}