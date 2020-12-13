using WebmilioCommons.Hooks.Wiring;
using WebmilioCommons.Hooks.World;

namespace WebmilioCommons.Hooks
{
    public static class Hooking
    {
        internal static void Load()
        {
            WorldHooks.Load();
        }

        internal static void PostSetupContent()
        {
            WorldHooks.PostSetupContent();
        }

        internal static void Unload()
        {
            WorldHooks.Unload();
        }
    }
}