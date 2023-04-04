using System;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace WebCom.Extensions;

public static class ModExtensions
{
    public static IEnumerable<Type> Concrete<T>(this IEnumerable<Mod> mods)
    {
        foreach (var mod in mods)
        {
            foreach (var type in mod.Code.GetTypes().Concrete<T>())
            {
                yield return type;
            }
        }
    }

    /// <param name="source"></param>
    /// <returns>List of <see cref="Mod"/> whose <see cref="Mod.Code"/> are not null.</returns>
    public static IEnumerable<Mod> GetNonNullAssemblyMods(this IList<Mod> source)
    {
        foreach (var mod in source)
        {
            if (mod.Code != null)
            {
                yield return mod;
            }
        }
    }
}
