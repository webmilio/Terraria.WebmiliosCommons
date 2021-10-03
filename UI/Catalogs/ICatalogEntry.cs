using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.Bestiary;

namespace WebmilioCommons.UI.Catalogs
{
    public interface ICatalogEntry
    {
        public void Draw(SpriteBatch spriteBatch, EntryIconDrawSettings settings);

        public bool Unlocked { get; set; }
        public string GetSearchString { get; }

        public IList<ICatalogEntryUIInfoProvider> UIInfoProviders { get; }
    }
}