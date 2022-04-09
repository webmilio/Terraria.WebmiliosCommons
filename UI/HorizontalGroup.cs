using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using WebmilioCommons.Extensions;

namespace WebmilioCommons.UI;

public class HorizontalGroup : UIPanel
{
    public HorizontalGroup AddElement(UIElement element)
    {
        Append(element);

        float width = 1f / Elements.Count;

        Elements.Do(delegate(UIElement uiElement, int index)
        {
            uiElement.Width = new(0, width);
            uiElement.Height = StyleDimension.Fill;

            uiElement.Left =  new(0, index * width);
        });

        return this;
    }
}