namespace WebCom.UI.Catalog;

public interface ICatalogEntry
{
    public ICatalogEntryIcon Icon { get; }
}

public class CatalogEntry : ICatalogEntry
{
    public ICatalogEntryIcon Icon { get; init; }
}