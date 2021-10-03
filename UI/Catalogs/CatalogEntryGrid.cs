using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.UI;
using WebmilioCommons.Extensions;

namespace WebmilioCommons.UI.Catalogs
{
    public abstract class CatalogEntryGrid : UIElement
    {
        protected int entryIndex, lastIndex;

        protected CatalogEntryGrid()
        {
            Width = new StyleDimension(0, 1);
            Height = new StyleDimension(0, 1);
        }

        public abstract void UpdateIndexes();

        public abstract void FillGridSpaceWithEntries();

        public void GetEntriesToShow(out int maxEntriesWidth, out int maxEntriesHeight, out int maxEntriesToHave)
        {
            Rectangle rectangle = GetDimensions().ToRectangle();

            maxEntriesWidth = rectangle.Width / EntryWidth;
            maxEntriesHeight = rectangle.Height / EntryHeight;
            maxEntriesToHave = maxEntriesWidth * maxEntriesHeight;
        }

        public void MakeButtonGoByOffset(UIElement element, int pagesCount)
        {
            element.OnClick += delegate
            {
                OffsetByPages(pagesCount);
            };
        }

        public void OffsetByPages(int pagesCount)
        {
            GetEntriesToShow(out _, out _, out int entryCount);
            FixRange(pagesCount * entryCount, entryCount);
            FillGridSpaceWithEntries();
        }


        // TODO Establish what this does cause I can't tell.
        public void FixRange(int offset, int entryCount)
        {
            entryIndex = Utils.Clamp(entryIndex + offset, 0, Math.Max(0, lastIndex - entryCount));
            OnGridContentsChanged?.Invoke();
        }

        public string GetPaginationText()
        {
            GetEntriesToShow(out _, out _, out int entryCount);

            int upper = Math.Min(lastIndex, entryIndex + entryCount);
            int lower = Math.Min(entryIndex, upper);

            return $"{upper}-{lower} ({lastIndex})";
        }

        public virtual int EntryWidth => 72;
        public virtual int EntryHeight => EntryWidth;

        public event Action OnGridContentsChanged;
    }

    public class CatalogEntryGrid<T> : CatalogEntryGrid where T : ICatalogEntry<T>
    {
        public delegate void GridEntryClickedEvent(UIMouseEvent evt, UIElement element, T entry);

        private readonly T[] _entries;
        private CatalogEntryButton<T>[] _buttons;

        public CatalogEntryGrid(IEnumerable<T> entries)
        {
            _entries = entries.ToArray();
        }

        public override void UpdateIndexes()
        {
            lastIndex = _entries.Length;
        }

        public override void FillGridSpaceWithEntries()
        {
            RemoveAllChildren();
            UpdateIndexes();

            GetEntriesToShow(out var maxWidth, out var maxHeight, out var maxEntriesCount);
            FixRange(0, maxEntriesCount);

            var displayCount = Math.Min(lastIndex, maxEntriesCount);
            var entries = new T[Math.Min(displayCount, displayCount - base.entryIndex)];

            CleanupHooks();

            Array.Copy(_entries, base.entryIndex, entries, 0, entries.Length);

            int entryIndex = 0;
            float
                w = 0.5f / maxWidth,
                h = 0.5f / maxHeight;

            _buttons = new CatalogEntryButton<T>[entries.Length];

            for (int i = 0; i < maxHeight; i++)
            {
                for (int j = 0; j < maxWidth && entryIndex < entries.Length; j++)
                {
                    CatalogEntryButton<T> button = _buttons[entryIndex] = new(new ContainedCatalogEntry<T>(entries[entryIndex++], entryIndex))
                    {
                        Top = new(0, (float)i / maxHeight - 0.5f + h),
                        Left = new(0, (float)j / maxWidth - 0.5f + w),

                        HAlign = VAlign = 0.5f
                    };

                    button.OnEntryClicked += OnEntryClicked;

                    button.SetSnapPoint("Entries", entryIndex, new Vector2(0.2f, 0.7f));
                    Append(button);
                }
            }
        }

        public override void OnActivate()
        {
            FillGridSpaceWithEntries();
        }

        public override void OnDeactivate()
        {
            CleanupHooks();
        }

        private void CleanupHooks()
        {
            if (_buttons != null)
            {
                _buttons.Do(b => b.OnEntryClicked -= OnEntryClicked);
            }
        }

        private void OnEntryClicked(UIMouseEvent evt, UIElement element, T entry)
        {
            GridEntryClicked?.Invoke(evt, element, entry);
        }

        public event GridEntryClickedEvent GridEntryClicked;
    }
}