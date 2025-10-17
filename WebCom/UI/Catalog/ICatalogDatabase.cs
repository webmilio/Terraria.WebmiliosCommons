using System.Collections.Generic;

namespace WebCom.UI.Catalog;

public interface ICatalogDatabase
{
    public IList<ICatalogEntry> GetEntries();
}
