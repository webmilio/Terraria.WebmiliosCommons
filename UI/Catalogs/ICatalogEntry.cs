using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.Bestiary;

namespace WebmilioCommons.UI.Catalogs
{
    public interface ICatalogEntry
    {
        public void Update();

        public void Draw(SpriteBatch spriteBatch, EntryIconDrawSettings settings);

        public int ProgressState { get; set; }
        public bool Unlocked => ProgressState > 0;

        public string SearchString { get; }
        public string HoverText { get; }

        public IList<IEntryInfoProvider> UIInfoProviders { get; }
    }
}