using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.UI;
using WebmilioCommons.Extensions;

namespace WebmilioCommons.UI.Catalogs
{
    public class CatalogEntryGrid : UIElement
    {
        protected int entryIndex, lastIndex;

        private readonly ICatalogEntry[] _entries;
        private readonly ICatalogEntry[] _workingEntries;
        private CatalogEntryButton[] _buttons;

        public CatalogEntryGrid(IEnumerable<ICatalogEntry> entries)
        {
            _entries = entries.ToArray();
            Width = StyleDimension.Fill;
            Height = StyleDimension.Fill;
        }

        public void UpdateIndexes()
        {
            lastIndex = _workingEntries.Length;
        }

        public void FillGridSpaceWithEntries()
        {
            RemoveAllChildren();
            UpdateIndexes();

            GetEntriesToShow(out var maxWidth, out var maxHeight, out var maxEntries);
            FixRange(0, maxEntries);

            var newSize = Math.Min(lastIndex, entryIndex + maxEntries);
            var entries = new ICatalogEntry[newSize];

            CleanupHooks();

            Array.Copy(_entries, entryIndex, entries, 0, entries.Length);

            int cIndex = 0;
            float
                w = 0.5f / maxWidth,
                h = 0.5f / maxHeight;

            _buttons = new CatalogEntryButton[entries.Length];

            for (int i = 0; i < maxHeight; i++)
            {
                for (int j = 0; j < maxWidth && cIndex < entries.Length; j++)
                {
                    CatalogEntryButton button = _buttons[cIndex] = new(new ContainedCatalogEntry(entries[cIndex++], entryIndex + cIndex))
                    {
                        Top = new(0, (float)i / maxHeight - 0.5f + h),
                        Left = new(0, (float)j / maxWidth - 0.5f + w),

                        HAlign = VAlign = 0.5f
                    };

                    button.OnEntryClicked += OnEntryClicked;

                    button.SetSnapPoint("Entries", cIndex, new Vector2(0.2f, 0.7f));
                    Append(button);
                }
            }
        }

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
        public void FixRange(int offset, int maxEntries)
        {
            var value = entryIndex + offset;
            var max = Math.Max(0, lastIndex - maxEntries);

            entryIndex = Utils.Clamp(value, 0, max);
            GridContentsChanged?.Invoke();
        }

        public string GetPaginationText()
        {
            GetEntriesToShow(out _, out _, out int entryCount);

            int upper = Math.Min(lastIndex, entryIndex + entryCount);
            int lower = Math.Min(entryIndex, upper);

            return $"{lower}-{upper} ({lastIndex})";
        }

        public override void Recalculate()
        {
            base.Recalculate();
            FillGridSpaceWithEntries();
        }

        public override void OnActivate()
        {
            //FillGridSpaceWithEntries();
        }

        public override void OnDeactivate()
        {
            CleanupHooks();
        }

        private void CleanupHooks()
        {
            if (_buttons != null)
            {
                _buttons.Do(b =>
                {
                    if (b == null)
                        return; // Safety check

                    b.OnEntryClicked -= OnEntryClicked;
                });
            }
        }

        private void OnEntryClicked(UIMouseEvent evt, UIElement element, ICatalogEntry entry)
        {
            GridEntryClicked?.Invoke(evt, element, entry);
        }

        public virtual int EntryWidth => 72;
        public virtual int EntryHeight => EntryWidth;

        public delegate void GridEntryClickedEvent(UIMouseEvent evt, UIElement element, ICatalogEntry entry);
        public event GridEntryClickedEvent GridEntryClicked;
        public event Action GridContentsChanged;
    }
}