using System.Collections.Generic;
using Terraria.UI;

namespace WebmilioCommons.UI.Catalogs
{
    public interface ICatalogEntryUIInfoProvider
    {
        public UIElement[] Provide(ICatalogEntry entry);
    }
}