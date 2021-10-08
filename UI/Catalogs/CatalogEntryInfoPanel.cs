using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace WebmilioCommons.UI.Catalogs
{
    public class CatalogEntryInfoPanel : UIPanel
    {
        protected UIList list;
        protected UIScrollbar scrollbar;

        public CatalogEntryInfoPanel()
        {
            var dimensions = CreationDimensions;

            Width = dimensions.width;
            Height = dimensions.height;
            SetPadding(0);

            BorderColor = CreationBorderColor;
            BackgroundColor = CreationBackgroundColor;

            list = new()
            {
                Width = new(-20f, 1f),
                Height = StyleDimension.Fill
            };

            list.SetPadding(2);
            list.PaddingBottom = list.PaddingTop = list.ListPadding = 4;

            Append(list);

            list.ManualSortMethod = ManualSortMethod; // TODO Verify necessity.

            scrollbar = new()
            {
                Left = new(-6, 0),
                Height = new(-20, 1),

                HAlign = 1,
                VAlign = 0.5f
            };
            scrollbar.SetView(100, 1000);
            list.SetScrollbar(scrollbar);

            Append(scrollbar);
            AppendBorder();
        }

        private void AppendBorder()
        {
            UIPanel panel = new()
            {
                Width = new StyleDimension(0f, 1f),
                Height = new StyleDimension(0f, 1f),
                IgnoresMouseInteraction = true,

                BorderColor = new Color(89, 116, 213, 255),
                BackgroundColor = Color.Transparent
            };

            Append(panel);
        }

        public void FillInfoForEntry(ICatalogEntry entry)
        {
            PopulateList(list, entry);
            Recalculate();
        }

        public static void PopulateList(UIList list, ICatalogEntry entry)
        {
            list.Clear();
            var providers = entry.UIInfoProviders;

            int categoryIndex = 0;

            for (int i = 0; i < providers.Count; i++)
            {
                var provider = providers[i];
                var element = provider.Provide(entry);

                if (categoryIndex != provider.CategoryIndex && element != null)
                {
                    categoryIndex = provider.CategoryIndex;

                    UIHorizontalSeparator separator = new()
                    {
                        Width = StyleDimension.Fill,
                        Color = new Color(89, 116, 213, 255) * 0.9f
                    };

                    list.Add(separator);
                }
                
                if (element != null)
                    list.Add(element);
            }
        }

        public virtual void ManualSortMethod(List<UIElement> list) { }

        public virtual (StyleDimension width, StyleDimension height) CreationDimensions { get; } = (new(230, 0), StyleDimension.Fill);

        public virtual Color CreationBorderColor { get; } = new(89, 116, 213, 255);
        public virtual Color CreationBackgroundColor { get; } = new(73, 94, 171);

        public virtual Color BorderBorderColor { get; } = new(89, 116, 213, 255);
        public virtual Color BorderBackgroundColor { get; } = Color.Transparent;
    }
}