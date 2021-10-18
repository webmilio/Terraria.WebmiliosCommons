using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;

namespace WebmilioCommons.UI;

public class UIRectanglePanel : UIPanel
{
    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        var dimensions = GetDimensions();

        spriteBatch.Draw(TextureAssets.BlackTile.Value, dimensions.ToRectangle(), BackgroundColor);
    }
}