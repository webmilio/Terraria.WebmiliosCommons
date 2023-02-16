using Terraria.ModLoader.UI;
using Terraria.UI;

namespace WebmilioCommons.UI;

public class UILabeledPanel<T> : DragableUIPanel
{
	public UILabeledPanel(T label, 
		float textMaxScale = 1, bool large = false)
	{
		var text = new UIAutoScaleTextTextPanel<T>(label, textMaxScale, large)
		{
			Width = StyleDimension.Fill,
			Height = new(.1f, 0)
		};
		Append(text);

		var container = new DragableUIPanel()
		{
			Height = new(0, .9f),
			Width = StyleDimension.Fill,

			Top = new(0, .1f)
		};
		Append(container);
	}
}
