using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent.Bestiary;

namespace WebmilioCommons.UI.Catalogs
{
    public abstract class CatalogEntry : ICatalogEntry
    {
        public virtual void Update() { }

        public virtual void Draw(SpriteBatch spriteBatch, EntryIconDrawSettings settings)
        {
            var spriteSize = Sprite.Size();
            spriteBatch.Draw(Sprite.Value, settings.iconbox.Center(), null, Color.White, 0f, spriteSize / 2, 1, SpriteEffects.None, 0);
        }

        public int ProgressState { get; set; }
        public virtual bool Unlocked => ProgressState > 0;

        public abstract string SearchString { get; }
        public virtual string HoverText => SearchString;

        public IList<IEntryInfoProvider> UIInfoProviders { get; set; }

        public virtual Asset<Texture2D> Sprite { get; protected set; }
    }
}