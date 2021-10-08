using System.Collections.Generic;
using Terraria.UI;

namespace WebmilioCommons.UI.Catalogs
{
    public interface IEntryInfoProvider
    {
        public UIElement Provide(ICatalogEntry entry);

        public int CategoryIndex { get; }
    }
}