using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using WebCom.DependencyInjection;
using WebCom.Extensions;
using WebCom.Resolvers;

namespace WebCom.Making;

[Service]
public class ModStore
{
    private readonly ModsProvider _mods;
    private readonly Dictionary<Type, ReadOnlyCollection<Type>> _types = new();

    public ModStore(ModsProvider mods)
    {
        _mods = mods;
    }

    public ReadOnlyCollection<Type> OfType<T>()
    {
        if (!_types.ContainsKey(typeof(T)))
        {
            var types = new List<Type>();

            foreach (var mod in _mods.Provide())
            {
                types.AddRange(mod.Concrete<T>());
            }

            _types.Add(typeof(T), types.AsReadOnly());
        }

        return _types[typeof(T)];
    }
}
