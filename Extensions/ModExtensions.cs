using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria.ModLoader;

namespace WebCom.Extensions;

public static class ModExtensions
{
    public static IEnumerable<Type> Concrete<T>(this IEnumerable<Mod> mods)
    {
        foreach (var mod in mods)
        {
            foreach (var type in Concrete<T>(mod))
            {
                yield return type;
            }
        }
    }

    public static IEnumerable<Type> Concrete<T>(this Mod mod) => mod.Code.GetTypes().Concrete<T>();
    public static IEnumerable<Type> Concrete(this Mod mod, Type type) => mod.Code.GetTypes().Concrete(type);

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

    /// <param name="source"></param>
    /// <returns>List of <see cref="Mod"/> whose <see cref="Mod.Code"/> are not null.</returns>
    public static IEnumerable<Assembly> GetNonNullAssemblies(this IList<Mod> source)
    {
        foreach (var mod in source)
        {
            if (mod.Code != null)
            {
                yield return mod.Code;
            }
        }
    }
}
