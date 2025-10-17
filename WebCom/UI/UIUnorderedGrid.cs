using System.Reflection;
using Terraria.ModLoader.UI.Elements;
using Terraria.UI;

namespace Gambling.Common.UIs;

public class UIUnorderedGrid : UIGrid
{
    private FieldInfo _field;
    protected UIElement innerList;

    public UIUnorderedGrid()
    {
        _field = typeof(UIGrid).GetField("_innerList", BindingFlags.NonPublic | BindingFlags.Instance);
        innerList = _field.GetValue(this) as UIElement;
    }

    public override void Add(UIElement item)
    {
        _items.Add(item);

        innerList.Append(item);
        innerList.Recalculate();
    }

    public override bool Remove(UIElement item)
    {
        innerList.RemoveChild(item);
        return _items.Remove(item);
    }
}
