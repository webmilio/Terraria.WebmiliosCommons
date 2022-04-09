using System;
using System.Collections.Generic;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader.UI.Elements;
using Terraria.UI;
using WebmilioCommons.Extensions;

namespace WebmilioCommons.UI.Browsers;

public class Browser<TElement, TValue> : UIPanel where TElement : UIElement
{
    private readonly IList<TElement> _elements;
    private readonly Func<TElement, TValue> _selector;
    private readonly UIGrid _grid;
    
    public Browser(IList<TElement> elements, Func<TElement, TValue> selector)
    {
        _elements = elements;
        _selector = selector;

        Append(_grid = new()
        {
            Width = StyleDimension.Fill,
            Height = StyleDimension.Fill,

            IgnoresMouseInteraction = false
        });

        _grid.AddRange(elements);
        elements.Do(e => e.OnClick += Element_OnClick);
    }

    /// <summary />
    ~Browser()
    {
        _elements.Do(e => e.OnClick -= Element_OnClick);
    }

    private void Element_OnClick(UIMouseEvent evt, UIElement element)
    {
        var t = (TElement) element;
        Selected = _selector(t);
    }

    public TValue Selected { get; private set; }
}