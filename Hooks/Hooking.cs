using WebmilioCommons.Hooks.Wiring;

namespace WebmilioCommons.Hooks
{
    public static class Hooking
    {
        internal static void Load()
        {
            WorldHooksProxy.Load();
        }

        internal static void PostSetupContent()
        {
            WorldHooksProxy.PostSetupContent();
        }

        internal static void Unload()
        {
            WorldHooksProxy.Unload();
        }
    }
}