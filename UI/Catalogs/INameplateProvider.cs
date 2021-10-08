using Terraria.GameContent.UI.Elements;

namespace WebmilioCommons.UI.Catalogs
{
    public interface INameplateProvider : IEntryInfoProvider
    {
        public static UIText LockedName { get; } = new("???");
    }
}