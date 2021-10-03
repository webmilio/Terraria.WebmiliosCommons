using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.Bestiary;

namespace WebmilioCommons.UI.Catalogs
{
    public abstract class CatalogEntry<T> : ICatalogEntry<T>
    {
        public abstract void Draw(SpriteBatch spriteBatch, EntryIconDrawSettings settings);

        public bool Unlocked { get; set; }

        public abstract string GetSearchString { get; }

        public IList<ICatalogEntryUIInfoProvider<T>> UIInfoProviders { get; set; }
    }
}