using System;
using System.Collections.Generic;
using Terraria.ModLoader;
using WebCom.Extensions;

namespace WebCom.Resolvers;

public class TypeProvider<T> : IProvider<Type>
{
    private readonly IEnumerable<Mod> _mods;

    public TypeProvider(IEnumerable<Mod> mods)
    {
        _mods = mods;
    }

    public IEnumerable<Type> Provide()
    {
        foreach (var mod in _mods)
        {
            foreach (var type in mod.Code?.GetTypes().Concrete<T>())
            {
                yield return type;
            }
        }
    }
}