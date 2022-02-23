using Terraria.UI;

namespace WebmilioCommons.UI;

public class UILayer : GameInterfaceLayer
{
    public UILayer(UIState state, string name, InterfaceScaleType scaleType) : base(name, scaleType)
    {
        State = state;
    }

    

    public UIState State { get; }

    public bool Active { get; private set; }
}