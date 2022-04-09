using Terraria.UI;

namespace WebmilioCommons.Extensions;

public static class UIElementExtensions
{
    public static T SetPaddingFluid<T>(this T element, float padding) where T : UIElement
    {
        element.SetPadding(padding);
        return element;
    }
}