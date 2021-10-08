using Terraria.UI;

namespace WebmilioCommons.UI.Catalogs
{
    public abstract class FilterInfoProvider : IEntryInfoProvider
    {
        protected FilterInfoProvider()
        {
            
        }

        public abstract UIElement Provide(ICatalogEntry entry);

        public int CategoryIndex { get; }
    }
}