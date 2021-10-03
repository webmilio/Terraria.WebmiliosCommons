using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace WebmilioCommons.UI.Catalogs
{
    public record ContainedCatalogEntry<T>(T Entry, int Index);

    public class CatalogEntryButton : UIElement
    {
        // ReSharper disable once InconsistentNaming
        public const string
            __SlotPath = Catalog.BestiaryPath + "/Slot_",
            SlotBackPath = __SlotPath + "Back",
            SlotFrontPath = __SlotPath + "Front",
            SlotSelectionPath = __SlotPath + "Selection",
            SlotOverlayPath = __SlotPath + "Overlay";

        protected UIImage bordersGlow, bordersOverlay, borders;

        protected static UIImage MakeBorder(Asset<Texture2D> asset)
        {
            UIImage image = new(asset)
            {
                HAlign = 0.5f,
                VAlign = 0.5f,

                IgnoresMouseInteraction = true
            };

            return image;
        }

        public override void MouseOver(UIMouseEvent evt)
        {
            SoundEngine.PlaySound(12);

            RemoveChild(borders);
            RemoveChild(bordersOverlay);

            Append(borders);
            Append(bordersGlow);
        }

        public override void MouseOut(UIMouseEvent evt)
        {
            RemoveChild(borders);
            RemoveChild(bordersGlow);

            Append(bordersOverlay);
            Append(borders);
        }

        public bool ShowIndexes { get; set; } = true;

        #region Textures

        private static Asset<Texture2D> _slotBack;
        public static Asset<Texture2D> SlotBack => _slotBack ??= Main.Assets.Request<Texture2D>(SlotBackPath);

        private static Asset<Texture2D> _slotFront;
        public static Asset<Texture2D> SlotFront => _slotFront ??= Main.Assets.Request<Texture2D>(SlotFrontPath);

        private static Asset<Texture2D> _slotSelection;
        public static Asset<Texture2D> SlotSelection => _slotSelection ??= Main.Assets.Request<Texture2D>(SlotSelectionPath);

        private static Asset<Texture2D> _slotOverlay;
        public static Asset<Texture2D> SlotOverlay => _slotOverlay ??= Main.Assets.Request<Texture2D>(SlotOverlayPath);

        #endregion
    }

    public class CatalogEntryButton<T> : CatalogEntryButton where T : ICatalogEntry<T>
    {
        public CatalogEntryButton(ContainedCatalogEntry<T> entry)
        {
            Entry = entry.Entry;

            Width.Set(72, 0);
            Height.Set(72, 0);

            UIElement imageContainer = new()
            {
                Width = new(-4, 1),
                Height = new(-4, 1),

                IgnoresMouseInteraction = true,
                OverflowHidden = true,

                HAlign = VAlign = 0.5f
            };

            imageContainer.Append(new UIImage(SlotBack)
            {
                HAlign = VAlign = 0.5f
            });

            CatalogEntryIcon icon = new(Entry);
            imageContainer.Append(icon);

            Append(imageContainer);

            if (ShowIndexes)
            {
                Append(new UIText((entry.Index).ToString(), 0.9f)
                {
                    Left = new(10, 0),
                    Top = new(10, 0),

                    IgnoresMouseInteraction = true
                });
            }

            bordersGlow = MakeBorder(SlotSelection);
            borders = MakeBorder(SlotFront);

            bordersOverlay = MakeBorder(SlotOverlay);
            bordersOverlay.Color = Color.White * 0.6f;

            Append(bordersOverlay);
            Append(borders);

            OnClick += Click;
        }

        ~CatalogEntryButton()
        {
            OnClick -= Click;
        }

        private void Click(UIMouseEvent evt, UIElement element)
        {
            OnEntryClicked?.Invoke(evt, element, Entry);
        }

        private T Entry { get; }

        public delegate void OnClickEvent(UIMouseEvent evt, UIElement element, T entry);

        public event OnClickEvent OnEntryClicked;
    }
}