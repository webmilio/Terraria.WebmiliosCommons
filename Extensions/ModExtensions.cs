using System.Collections.Generic;
using System.Reflection;
using Terraria.ModLoader;

namespace WebCom.Extensions;

internal static class ModExtensions
{
    internal static List<Mod> GetNonNullAssemblies(this IList<Mod> source)
    {
        var mods = new List<Mod>(source.Count);

        foreach (var mod in source)
        {
            if (mod.Code != null)
            {
                mods.Add(mod);
            }
        }

        return mods;
    }
}
