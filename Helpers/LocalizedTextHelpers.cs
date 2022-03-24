using Terraria.ModLoader;

namespace WebmilioCommons.Helpers;

public static class LocalizedTextHelpers
{
    public static string GetModKey(Mod mod, string key) => GetModKey(mod.Name, key);

    public static string GetModKey(string modName, string key)
    {
        return $"Mods.{modName}.{key}";
    }
}