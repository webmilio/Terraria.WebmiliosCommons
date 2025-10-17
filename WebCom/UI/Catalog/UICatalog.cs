using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace WebCom.UI.Catalog;

// I don't know if this is necessary but obviously, I took most of this from vanilla Bestiary. Soo uuuh credits to your mom.
public class UICatalog : UIElement
{
    private const int TopBarHeight = 24;
    private const int ProgressBarHeight = 20;
    private const int InfoPageWidth = 230;

    private const int BackForwardButtonMargin = 1;

    public static readonly Color DefaultBeastiaryBlue = new Color(33, 43, 79) * 0.8f;

    private IList<ICatalogEntry> _originalEntries, _workingEntries;

    private UICatalogEntryGrid _grid;
    private UICatalogEntryInfoPage _info;
    private UIText _rangeText, _filterText, _sortText;

    private UICatalogParameters Parameters { get; }

    public UICatalog(UICatalogParameters parameters)
    {
        Parameters = parameters;

        _originalEntries = new List<ICatalogEntry>(Parameters.DataSource.GetEntries());
        _workingEntries = new List<ICatalogEntry>(_originalEntries);

        BuildRoot();
    }

    private void BuildRoot()
    {
        var root = new UIPanel
        {
            Width = StyleDimension.Fill,
            Height = StyleDimension.Fill
        };

        root.PaddingTop -= 4;
        root.PaddingBottom -= 4;

        Append(root);

        BuildGridContainer(root);
        BuildTopBar(root);
    }

    #region Top Bar
    private void BuildTopBar(UIElement root)
    {
        var topBar = new UIElement
        {
            Width = StyleDimension.Fill,
            Height = StyleDimension.FromPixels(TopBarHeight)
        };
        topBar.SetPadding(0);

        root.Append(topBar);

        BuildBackForward(topBar);
        BuildSortFilter(topBar);
    }

    private void BuildBackForward(UIElement container)
    {
        var buttonBorder = Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Button_Border");

        var back = new UIImageButton(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Button_Back"));
        back.SetHoverImage(buttonBorder);
        back.SetVisibility(1, 1);
        back.SetSnapPoint("BackPage", 0);
        _grid.MakeButtonOffset(back, -1);

        container.Append(back);

        var forward = new UIImageButton(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Button_Forward"))
        {
            Left = StyleDimension.FromPixels(back.Width.Pixels + BackForwardButtonMargin)
        };

        forward.SetHoverImage(buttonBorder);
        forward.SetVisibility(1, 1);
        forward.SetSnapPoint("NextPage", 0);
        _grid.MakeButtonOffset(forward, 1);

        container.Append(forward);

        var textPanel = new UIPanel
        {
            Left = StyleDimension.FromPixels(back.Width.Pixels + BackForwardButtonMargin + forward.Width.Pixels + BackForwardButtonMargin),

            Width = StyleDimension.FromPixels(135),
            Height = StyleDimension.Fill,

            VAlign = .5f,

            BackgroundColor = new Color(35, 40, 83),
            BorderColor = new Color(35, 40, 83)
        };
        textPanel.SetPadding(0);

        container.Append(textPanel);

        _rangeText = new UIText("undef-undef", .8f)
        {
            HAlign = .5f,
            VAlign = .5f,
        };
        _grid.OnGridContentsChanged += Grid_OnGridContentsChanged;

        textPanel.Append(_rangeText);
    }
    
    private void BuildSortFilter(UIElement container)
    {
        // Filtering
        var filterButton = new UIImageButton(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Button_Filtering"))
        {
            Left = StyleDimension.FromPixels(0 - _info.Width.Pixels - 17),

            HAlign = 1
        };

        filterButton.SetHoverImage(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Button_Wide_Border"));
        filterButton.SetVisibility(1, 1);
        filterButton.SetSnapPoint("FilterButton", 0);
        // filterButton.OnLeftClick += ToggleFilteringGrid;

        container.Append(filterButton);

        // Sorting
        _filterText = new UIText("%filter%", .8f)
        {
            Left = StyleDimension.FromPixels(34),
            Top = StyleDimension.FromPixels(2),

            VAlign = .5f,

            TextOriginX = 0,
            TextOriginY = 0
        };
        filterButton.Append(_filterText);

        var sortingButton = new UIImageButton(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Button_Sorting"))
        {
            Left = StyleDimension.FromPixels(0 - _info.Width.Pixels - filterButton.Width.Pixels -3 - 17),

            HAlign = 1
        };

        sortingButton.SetHoverImage(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Button_Wide_Border"));
        sortingButton.SetVisibility(1, 1);
        sortingButton.SetSnapPoint("SortButton", 0);
        // filterButton.OnLeftClick += ToggleSortingGrid;

        container.Append(sortingButton);

        // Merge initialization code with Filter's
        var sortText = new UIText("%sort%", .8f)
        {
            Left = StyleDimension.FromPixels(34),
            Top = StyleDimension.FromPixels(2),

            VAlign = .5f,

            TextOriginX = 0,
            TextOriginY = 0
        };
        sortingButton.Append(sortText);
    }
    #endregion

    private void BuildGridContainer(UIElement root)
    {
        var gridContainer = new UIElement
        {
            Width = StyleDimension.Fill,
            Height = StyleDimension.FromPixelsAndPercent(-TopBarHeight - 6 - ProgressBarHeight, 1f),

            VAlign = 1,

            Top = StyleDimension.FromPixels(-ProgressBarHeight)
        };
        gridContainer.SetPadding(0);

        root.Append(gridContainer);

        BuildEntryGrid(gridContainer);
        BuildEntryInfoPage(gridContainer);
    }

    private void BuildEntryGrid(UIElement container)
    {
        _grid = new UICatalogEntryGrid(_workingEntries, Click_SelectEntry)
        {
            Width = StyleDimension.FromPixelsAndPercent(-12 - InfoPageWidth, 1),
            Height = StyleDimension.FromPixelsAndPercent(-4, 1),

            VAlign = 1
        };

        container.Append(_grid);
    }

    private void BuildEntryInfoPage(UIElement container)
    {
        _info = new UICatalogEntryInfoPage()
        {
            Width = StyleDimension.FromPixels(230),
            Height = StyleDimension.FromPixelsAndPercent(12, 1),

            HAlign = 1,

            BorderColor = UICatalogEntryInfoPage.BestiaryDefaultBorderColor,
            BackgroundColor = UICatalogEntryInfoPage.BestiaryDefaultBackgroundColor
        };

        container.Append(_info);
    }

    private void Click_SelectEntry(UIMouseEvent evt, UIElement element)
    {
        // TODO: Fill this
    }

    private void Grid_OnGridContentsChanged()
    {
        _rangeText.SetText(_grid.GetRangeText());
    }
}
