using System;
using System.Collections.Generic;
using Terraria;
using Terraria.UI;

namespace WebCom.UI.Catalog;

public class UICatalogEntryGrid : UIElement
{
    private IList<ICatalogEntry> _entries;
    private MouseEvent _entryClick;

    private int _atEntryIndex, _lastEntry;

    public event Action OnGridContentsChanged;

    public UICatalogEntryGrid(IList<ICatalogEntry> workingSet, MouseEvent entryClick)
    {
        _entries = workingSet;
        _entryClick = entryClick;

        SetPadding(0);
        FillWithEntries();
    }

    public void UpdateLastEntry()
    {
        _lastEntry = _entries.Count;
    }

    public void FillWithEntries()
    {
        RemoveAllChildren();
        
        UpdateLastEntry();
        var maxes = GetEntriesToShow();
        FixRange(0, maxes.Entries);

        var atEntryIndex = _atEntryIndex;
        var lastEntry = Math.Min(_lastEntry, atEntryIndex + maxes.Entries);

        var entries = new List<ICatalogEntry>();

        for (int i = atEntryIndex; i < lastEntry; i++)
        {
            entries.Add(_entries[i]);
        }

        var offsetX = .5f / maxes.Width;
        var offsetY = .5f / maxes.Height;

        for (int i = 0; i < maxes.Height; i++)
        {
            for (int j = 0; j < maxes.Width; j++)
            {
                int k = i * maxes.Width + j;

                if (k >= entries.Count)
                {
                    break;
                }

                var button = new UICatalogEntryButton(entries[k], prettyPortrait: false)
                {
                    Width = StyleDimension.FromPixels(72),
                    Height = StyleDimension.FromPixels(72),

                    VAlign = .5f,
                    HAlign = .5f,

                    Left = StyleDimension.FromPercent(j / (float)maxes.Width - .5f + offsetX),
                    Top = StyleDimension.FromPercent(i / (float)maxes.Height - .5f + offsetY)
                };

                button.SetSnapPoint("Entries", k, anchor: new(.2f, .7f));

                if (_entryClick != null)
                {
                    button.OnLeftClick += _entryClick;
                }

                Append(button);
            }
        }
    }

    public override void Recalculate()
    {
        base.Recalculate();

        FillWithEntries();
    }

    public record CalculatedEntries(int Width, int Height)
    {
        public int Entries => Width * Height;
    }

    public CalculatedEntries GetEntriesToShow()
    {
        var rectangle = GetDimensions().ToRectangle();

        var maxWidth = rectangle.Width / 72;
        var maxHeight = rectangle.Height / 72;

        return new(maxWidth, maxHeight);
    }

    private void FixRange(int offset, int maxEntries)
    {
        _atEntryIndex = Utils.Clamp(_atEntryIndex + offset, 0, Math.Max(0, _lastEntry - maxEntries));
        OnGridContentsChanged?.Invoke();
    }

    public string GetRangeText()
    {
        var maxes = GetEntriesToShow();
        var end = Math.Min(_lastEntry, _atEntryIndex + maxes.Entries);
        var start = Math.Min(_atEntryIndex + 1, end);

        return $"{start}-{end} ({_lastEntry})";
    }

    public void MakeButtonOffset(UIElement element, int pages)
    {
        element.OnLeftClick += delegate
        {
            OffsetByPages(pages);
        };
    }

    public void OffsetByPages(int pages)
    {
        var maxes = GetEntriesToShow();
        OffsetLibrary(pages * maxes.Entries);
    }

    public void OffsetLibrary(int offset)
    {
        var maxes = GetEntriesToShow();

        FixRange(offset, maxes.Entries);
        FillWithEntries();
    }
}
