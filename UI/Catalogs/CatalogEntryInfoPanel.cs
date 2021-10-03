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

        public void AppendBorder()
        {
            Append(new UIPanel
            {
                Width = new(0, 1),
                Height = new(0, 1),

                IgnoresMouseInteraction = true,

                BorderColor = BorderColor,
                BackgroundColor = BorderBackgroundColor
            });
        }

        public virtual void ManualSortMethod(List<UIElement> list) { }

        public virtual (StyleDimension width, StyleDimension height) CreationDimensions { get; } = (new(230, 0), new(0, 1));

        public virtual Color CreationBorderColor { get; } = new(89, 116, 213, 255);
        public virtual Color CreationBackgroundColor { get; } = new(73, 94, 171);

        public virtual Color BorderBorderColor { get; } = new(89, 116, 213, 255);
        public virtual Color BorderBackgroundColor { get; } = Color.Transparent;
    }

    public class CatalogEntryInfoPanel<T> : CatalogEntryInfoPanel where T : ICatalogEntry<T>
    {
        public CatalogEntryInfoPanel()
        {
            var dimensions = CreationDimensions;

            Width = dimensions.width;
            Height = dimensions.height;

            BorderColor = CreationBorderColor;
            BackgroundColor = CreationBackgroundColor;

            list = new()
            {
                Width = new(0, 1),
                Height = new(0, 1)
            };

            list.SetPadding(2);
            list.PaddingBottom = list.PaddingTop = list.ListPadding = 4;
            
            Append(list);

            list.ManualSortMethod = ManualSortMethod; // TODO Verify necessity.

            scrollbar = new()
            {
                Height = new(-20, 1),
                Left = new(-6, 0),

                HAlign = 1,
                VAlign = 0.5f
            };
            scrollbar.SetView(100, 1000);
            list.SetScrollbar(scrollbar);

            Append(scrollbar);
            AppendBorder();
        }

        public void FillInfoForEntry(T entry)
        {
            PopulateList(list, entry);
            Recalculate();
        }

        public static void PopulateList(UIList list, T entry)
        {
            list.Clear();
            var providers = entry.UIInfoProviders;

            for (int i = 0; i < providers.Count; i++)
            {
                var elements = providers[i].Provide(entry);

                for (int j = 0; j < elements.Length; j++)
                {
                    list.Add(elements[i]);
                }

                if (i + 1 < providers.Count)
                {
                    UIHorizontalSeparator separator = new()
                    {
                        Width = StyleDimension.Fill,
                        Color = new Color(89, 116, 213, 255) * 0.9f
                    };

                    list.Add(separator);
                }
            }
        }
    }
}