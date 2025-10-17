using Microsoft.Xna.Framework;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace WebCom.UI.Catalog;

public class UICatalogEntryInfoPage : UIPanel
{
    public static readonly Color BestiaryDefaultBackgroundColor = new(73, 94, 171);
    public static readonly Color BestiaryDefaultBorderColor = new(89, 116, 213, 255);

    private readonly UIList _infoList;
    private readonly UIScrollbar _scrollbar;

    public UICatalogEntryInfoPage()
    {
        SetPadding(0);

        _infoList = new()
        {
            Width = StyleDimension.FromPercent(1),
            Height = StyleDimension.FromPercent(1),

            ListPadding = 4,

            PaddingTop = 4,
            PaddingBottom = 4
        };
        _infoList.SetPadding(2);

        Append(_infoList);

        _scrollbar = new()
        {
            Height = StyleDimension.FromPixelsAndPercent(-20, 1),

            Left = StyleDimension.FromPixels(-6),

            HAlign = 1,
            VAlign = .5f
        };
        _scrollbar.SetView(100, 1000);
        _infoList.SetScrollbar(_scrollbar);

        Append(_scrollbar);
        CheckScrollbar();
    }

    private bool _isScrollbarAttached;
    private void CheckScrollbar()
    {
        if (_isScrollbarAttached)
        {
            RemoveChild(_scrollbar);

            _isScrollbarAttached = false;
            _infoList.Width.Set(0, 1);
        }
        else
        {
            Append(_scrollbar);

            _isScrollbarAttached = true;
            _infoList.Width.Set(-20, 1);
        }

        /*
         * The original code goes like this, and it makes no sense cause canScroll is always true.
         * if (_scrollbar != null) {
		    bool canScroll = _scrollbar.CanScroll;
		    canScroll = true;
		    if (_isScrollbarAttached && !canScroll) {
			    RemoveChild(_scrollbar);
			    _isScrollbarAttached = false;
			    _list.Width.Set(0f, 1f);
		    }
		    else if (!_isScrollbarAttached && canScroll) {
			    Append(_scrollbar);
			    _isScrollbarAttached = true;
			    _list.Width.Set(-20f, 1f);
		    }
	    }
         */
    }

    // TODO Add remaining code to fill the panel with info.
}
