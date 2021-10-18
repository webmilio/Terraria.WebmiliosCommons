using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.UI;

namespace WebmilioCommons.UI.Catalogs
{
    public class NameplateUIInformationProvider : INameplateProvider
    {
        private readonly UIElement _unlocked;
        private readonly UIElement _locked = INameplateProvider.LockedName;

        public NameplateUIInformationProvider()
        {
        }

        public NameplateUIInformationProvider(string name)
        {
            _unlocked = MakeUILabel(new UIText(name));
        }

        public NameplateUIInformationProvider(LocalizedText localized)
        {
            _unlocked = MakeUILabel(new UIText(localized));
        }

        private UIElement MakeUILabel(UIText label)
        {
            label.HAlign = label.VAlign = 0.5f;
            label.Top = new StyleDimension(2f, 0f);
            label.IgnoresMouseInteraction = true;

            UIElement labelContainer = new()
            {
                Width = StyleDimension.Fill,
                Height = new(24, 0)
            };
            labelContainer.Append(label);

            return labelContainer;
        }

        public UIElement Provide(ICatalogEntry entry)
        {
            return entry.Unlocked ? _unlocked : _locked;
        }

        public int CategoryIndex { get; } = 0;
    }
}