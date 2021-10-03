using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.UI;

namespace WebmilioCommons.UI.Catalogs
{
    public class CatalogEntryIcon : UIElement
    {
        public const string EntryLockedPath = Catalog.BestiaryPath + "/Icon_Locked";

        public CatalogEntryIcon(ICatalogEntry entry) : this(entry, false) { }

        public CatalogEntryIcon(ICatalogEntry entry, bool isPortrait)
        {
            Entry = entry ?? throw new ArgumentException($"Tried creating a {nameof(CatalogEntryIcon)} with no entry.");
            IsPortrait = isPortrait;

            OverrideSamplerState = SamplerState;
            UseImmediateMode = true; // TODO Add override.

            Width.Set(0, 1);
            Height.Set(0, 1);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            CalculatedStyle dimensions = GetDimensions();

            if (Entry.Unlocked)
            {
                Entry.Draw(spriteBatch, new()
                {
                    iconbox = dimensions.ToRectangle(),
                    IsHovered = IsMouseHovering
                });
            }
            else
            {
                spriteBatch.Draw(LockedTexture.Value, dimensions.Center(), null, Color.White * 0.15f, 0, LockedTexture.Value.Size() / 2, 1, SpriteEffects.None, 0);
            }
        }

        public ICatalogEntry Entry { get; }
        public bool IsPortrait {  get; }

        public virtual SamplerState SamplerState { get; } = Main.DefaultSamplerState;

        private static Asset<Texture2D> _lockedTexture;
        public static Asset<Texture2D> LockedTexture => _lockedTexture ??= Main.Assets.Request<Texture2D>(EntryLockedPath);
    }
}