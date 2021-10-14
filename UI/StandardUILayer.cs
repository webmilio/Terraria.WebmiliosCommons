using Terraria;
using Terraria.UI;

namespace WebmilioCommons.UI
{
    public class StandardUILayer : GameInterfaceLayer
    {
        public StandardUILayer(StandardUIState uiState, string layerName, InterfaceScaleType scaleType) : base(layerName, scaleType)
        {
            UIState = uiState;
        }

        protected override bool DrawSelf()
        {
            if (UIState.Visible)
            {
                UIState.Draw(Main.spriteBatch);
            }

            return true;
        }


        public StandardUIState UIState { get; }
    }
}