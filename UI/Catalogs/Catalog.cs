using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.UI;

namespace WebmilioCommons.UI.Catalogs
{
    public class Catalog : StandardUIState
    {
#pragma warning disable 1591
        public const string
            BestiaryPath = "Images/UI/Bestiary",
            __BestiaryButton = BestiaryPath + "/Button_";

        public const string
            BackButtonPath = __BestiaryButton + "Back",
            NextButtonPath = __BestiaryButton + "Forward",
            ButtonBorderPath = __BestiaryButton + "Border",

            ButtonWideBorderPath = __BestiaryButton + "Wide_Border";

        public const string
            SearchButtonPath = __BestiaryButton + "Search",
            SearchButtonBorderPath = SearchButtonPath + "_Border";

        public const string
            FilterButtonPath = __BestiaryButton + "Filtering",
            SortButtonPath = __BestiaryButton + "Sorting";

        public const int
            StandardPadding = 20,
            SearchBarHeight = 24;
#pragma warning restore 1591

        private readonly ICatalogEntry[] _originalEntries;
        private readonly List<ICatalogEntry> _workingEntries;

        private UIText _rangeText;
        private CatalogEntryInfoPanel _infoPanel;
        private CatalogEntryGrid _entryGrid;

        public Catalog(ICatalogEntry[] entries)
        {
            _originalEntries = entries;
            _workingEntries = new(_originalEntries);

            BuildCatalog();
        }

        public void BuildCatalog()
        {
            RemoveAllChildren();

            const int
                topPadding = 220,
                extraSize = 100,
                minWidth = 600 + extraSize,
                maxWidth = 800 + extraSize;

            UIPanel mainPanel;
            UIElement innerContainer;

            {
                UIElement root = new()
                {
                    Width = new(0, 0.875f),
                    Height = new(-topPadding, 1),

                    MinWidth = new(minWidth, 0),
                    MaxWidth = new(maxWidth, 0),

                    Top = new(topPadding, 0),
                    HAlign = 0.5f
                };
                Append(root);

                mainPanel = new()
                {
                    Width = new StyleDimension(0, 1),
                    Height = new StyleDimension(-90, 1),

                    BackgroundColor = new Color(33, 43, 79) * 0.8f
                };

                root.Append(mainPanel);
                root.Append(MakeExitButton());

                mainPanel.PaddingTop -= 4;
                mainPanel.PaddingBottom -= 4;

                innerContainer = new()
                {
                    Width = new(0, 1),
                    Height = new(-StandardPadding - 6 - SearchBarHeight, 1),

                    Top = new(-StandardPadding, 0),
                    VAlign = 1
                };

                mainPanel.Append(innerContainer);
            }

            {
                _infoPanel = new CatalogEntryInfoPanel
                {
                    Height = new(12, 1),
                    HAlign = 1
                };
            }

            {
                UIElement entryContainer = new()
                {
                    Width = new(-12 - _infoPanel.Width.Pixels, 1),
                    Height = new(-4, 1),

                    VAlign = 1
                };

                innerContainer.Append(entryContainer);

                _entryGrid = new CatalogEntryGrid(_originalEntries);
                _entryGrid.GridEntryClicked += EntryGrid_OnEntryClicked;
                _entryGrid.GridContentsChanged += EntryGrid_OnContentsChanged;

                entryContainer.Append(_entryGrid);
                innerContainer.Append(_infoPanel);
            }

            var topBar = MakeTopBar(_entryGrid, _infoPanel);
            mainPanel.Append(topBar.bar);

            _rangeText = topBar.rangeText;

            if (HasProgressBar)
            {
                var progressBar = MakeProgressBar();
                mainPanel.Append(progressBar);

                //MakeProgressText(progressBar);
            }
        }

        ~Catalog()
        {
            _entryGrid.GridEntryClicked -= EntryGrid_OnEntryClicked;
            _entryGrid.GridContentsChanged -= EntryGrid_OnContentsChanged;
        }

        #region Top Bar

        public static (UIElement bar, UIText rangeText) MakeTopBar(CatalogEntryGrid grid, CatalogEntryInfoPanel infoPanel)
        {
            UIElement topBar = new()
            {
                Width = new StyleDimension(0, 1),
                Height = new StyleDimension(SearchBarHeight, 0)
            };

            topBar.SetPadding(0);

            var navigationButtons = AddNavigationButtons(topBar, grid);
            AddSortAndFilterButtons(topBar, -infoPanel.Width.Pixels);
            AddSearchContainer(topBar, -infoPanel.Width.Pixels);

            return (topBar, navigationButtons.Item3.textRange);
        }

        public static UIImageButton MakeButton(Asset<Texture2D> image, Asset<Texture2D> hover, string snapPoint)
        {
            var button = new UIImageButton(image);
            button.SetHoverImage(hover);
            button.SetVisibility(1, 1);
            button.SetSnapPoint(snapPoint, 0);

            return button;
        }

        #region Navigation Buttons

        public static (UIImageButton back, UIImageButton next, (UIPanel textPanel, UIText textRange)) AddNavigationButtons(UIElement container, CatalogEntryGrid grid)
        {
            var hoverImage = Main.Assets.Request<Texture2D>(ButtonBorderPath);

            var back = MakeNavigationButton(Main.Assets.Request<Texture2D>(BackButtonPath),
                hoverImage, "BackPage");
            NavigationButtonAction(back, grid, -1);

            var next = MakeNavigationButton(Main.Assets.Request<Texture2D>(NextButtonPath),
                hoverImage, "NextPage");
            NavigationButtonAction(next, grid, 1);

            next.Left = new(back.Width.Pixels + 1, 0);

            container.Append(back);
            container.Append(next);

            var text = MakeRangeText(back.Width.Pixels + next.Width.Pixels + 4);
            container.Append(text.textPanel);

            return (back, next, text);
        }

        public static UIImageButton MakeNavigationButton(Asset<Texture2D> image, Asset<Texture2D> hover, string snapPoint)
        {
            return MakeButton(image, hover, snapPoint);
        }

        public static void NavigationButtonAction(UIImageButton button, CatalogEntryGrid grid, int direction)
        {
            grid.MakeButtonGoByOffset(button, direction);
        }

        public static (UIPanel textPanel, UIText textRange) MakeRangeText(float xOffset)
        {
            UIPanel panel = new()
            {
                Left = new StyleDimension(xOffset, 0),

                Width = new StyleDimension(135, 0),
                Height = StyleDimension.Fill,

                VAlign = 0.5f,

                BackgroundColor = new(35, 40, 83),
                BorderColor = new(35, 40, 83)
            };

            UIText text = new("9000-9999 (9001)", 0.8f)
            {
                HAlign = 0.5f,
                VAlign = 0.5f
            };

            panel.Append(text);
            return (panel, text);
        }

        #endregion

        #region Sort & Filter Buttons

        public static void AddSortAndFilterButtons(UIElement container, float xOffset)
        {
            const int padding = 17;
            var hover = Main.Assets.Request<Texture2D>(ButtonWideBorderPath);

            var filter = MakeSortOrFilterButton(Main.Assets.Request<Texture2D>(FilterButtonPath), hover, "FilterButton", xOffset - padding);
            container.Append(filter);

            var sort = MakeSortOrFilterButton(Main.Assets.Request<Texture2D>(SortButtonPath), hover, "SortButton", xOffset - padding - filter.Width.Pixels - 3);
            container.Append(sort);
        }

        public static UIImageButton MakeSortOrFilterButton(Asset<Texture2D> image, Asset<Texture2D> hover, string snapPoint, float xOffset)
        {
            var button = MakeButton(image, hover, snapPoint);
            button.Left = new(xOffset, 0);
            button.HAlign = 1;

            return button;
        }

        #endregion

        #region Search

        public static void AddSearchContainer(UIElement container, float xOffset)
        {
            var search = MakeButton(Main.Assets.Request<Texture2D>(SearchButtonPath), Main.Assets.Request<Texture2D>(SearchButtonBorderPath), "SearchButton");
            search.Left = new(xOffset, 1f);
            search.VAlign = 0.5f;

            container.Append(search);
            container.Append(MakeSearchBar(search, xOffset));
        }

        public static UIElement MakeSearchBar(UIElement searchButton, float xOffset)
        {
            UIPanel panel = new()
            {
                Left = new(xOffset + searchButton.Width.Pixels + 3, 1),

                Width = new(-xOffset - searchButton.Width.Pixels - 3, 0),
                Height = StyleDimension.Fill,

                VAlign = 0.5f,

                BackgroundColor = new Color(35, 40, 83),
                BorderColor = new Color(35, 40, 83)
            };

            UISearchBar searchBar = new(Language.GetText("UI.PlayerNameSlot"), 0.8f)
            {
                Width = StyleDimension.Fill,
                Height = StyleDimension.Fill,

                Left = StyleDimension.Empty,

                HAlign = 0f,
                VAlign = 0.5f,

                IgnoresMouseInteraction = true
            };
            panel.Append(searchBar);

            // TODO Fix the search cancel.
            /*UIImageButton cancelSearch = new(Main.Assets.Request<Texture2D>("Images/UI/SearchCancel"))
            {
                Left = new(-2, 0),

                HAlign = 1,
                VAlign = 0.5f
            };
            panel.Append(cancelSearch);*/

            return panel;
        }

        #endregion

        #endregion

        #region Progress Bar

        public static UIElement MakeProgressBar()
        {
            UIElement progressBar = new()
            {
                Width = StyleDimension.Fill,
                Height = new(StandardPadding, 0),

                VAlign = 1
            };

            return progressBar;
        }

        public static UIText MakeProgressText()
        {
            return null;
        }

        #endregion

        #region Exit

        public static UIElement MakeExitButton()
        {
            UITextPanel<LocalizedText> text = new(Language.GetText("UI.Back"), 0.7f, true)
            {
                Width = StyleDimension.FromPixelsAndPercent(-10f, 0.5f),
                Height = new(50, 0),

                VAlign = 1,
                HAlign = 0.5f,

                Top = new(-25f, 0)
            };

            text.OnMouseOver += ExitButton_FadedMouseOver;
            text.OnMouseOut += ExitButton_FadedMouseOut;
            text.OnClick += ExitButton_Click;
            text.SetSnapPoint("ExitButton", 0);

            return text;
        }

        private static void ExitButton_Click(UIMouseEvent evt, UIElement element)
        {
            SoundEngine.PlaySound(11);

            if (Main.gameMenu)
                Main.menuMode = 0;
            else
                IngameFancyUI.Close();
        }

        private static void ExitButton_FadedMouseOver(UIMouseEvent evt, UIElement element)
        {
            var panel = (UIPanel)evt.Target;
            SoundEngine.PlaySound(12);

            // TODO Make moddable.
            panel.BackgroundColor = new Color(73, 94, 171);
            panel.BorderColor = Colors.FancyUIFatButtonMouseOver;
        }

        private static void ExitButton_FadedMouseOut(UIMouseEvent evt, UIElement element)
        {
            var panel = (UIPanel)evt.Target;

            // TODO Make moddable.
            panel.BackgroundColor = new Color(63, 82, 151) * 0.8f;
            panel.BorderColor = Color.Black;
        }

        #endregion

        private void EntryGrid_OnEntryClicked(UIMouseEvent evt, UIElement element, ICatalogEntry entry)
        {
            SelectEntry(entry);
        }

        private void EntryGrid_OnContentsChanged()
        {
            if (_rangeText == null)
                return;

            var text = _entryGrid.GetPaginationText();
            _rangeText.SetText(text);
        }

        public void SelectEntry(ICatalogEntry entry)
        {
            _infoPanel.FillInfoForEntry(entry);
        }

        public bool HasProgressBar { get; set; }
    }
}