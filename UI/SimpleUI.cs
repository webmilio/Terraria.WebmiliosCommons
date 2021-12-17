using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace WebmilioCommons.UI;

public class SimpleUI : ModSystem
{
    public static void Open(UIState state)
    {
        if (CurrentState != state)
        {
            CloseLast();
        }

        CurrentState = state;
        Main.InGameUI.SetState(CurrentState);
    }

    public static void CloseLast()
    {
        if (CurrentState != null && CurrentState == Main.InGameUI.CurrentState)
        {
            CurrentState = null;
            Main.InGameUI.SetState(null);
        }
    }

    public override void UpdateUI(GameTime gameTime)
    {
        if (Main.keyState[Keys.Escape] == KeyState.Down)
        {
            CloseLast();
        }
    }

    public static UIState CurrentState { get; private set; }
}