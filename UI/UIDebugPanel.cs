﻿using Microsoft.Xna.Framework;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace WebmilioCommons.UI
{
    public class UIDebugPanel : UIPanel
    {
        public UIDebugPanel()
        {
            Width = StyleDimension.Fill;
            Height = StyleDimension.Fill;

            BackgroundColor = Color.Red;
        }
    }
}