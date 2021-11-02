using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria.ModLoader;
using WebmilioCommons.Commons;
using WebmilioCommons.Extensions;

namespace WebmilioCommons.Loaders;

public abstract class Loader<T>
{
    /// <summary>Instantiates a new <see cref="Loader{T}"/> and loads all found subtypes of <see cref="T"/> that are not abstract.</summary>
    protected Loader() : this(typeInfo => true) { }

    /// <summary>
    /// Instantiates a new <see cref="Loader{T}"/> and loads all found subtypes of <see cref="T"/> that are not abstract and
    /// match the specified <see cref="loadCondition" />load condition(s).
    /// </summary>
    /// <param name="loadCondition">The condition under which a subclass of <see cref="T"/> should be loaded.</param>
    protected Loader(Func<TypeInfo, bool> loadCondition)
    {
        LoadCondition = loadCondition;
    }

    /// <summary>Finds and loads all instances of the specified type.</summary>
    public void Load()
    {
        PreLoad();

        foreach (Mod mod in ModStore.Mods)
        {
            foreach (TypeInfo type in mod.Code.Concrete<T>().Where(LoadCondition))
            {
                if (type.TryGetCustomAttribute(out SkipAttribute _))
                    continue;

                Load(mod, type);
            }
        }

        PostLoad();
    }

    /// <summary>Called at the beginning of <see cref="Load()"/>.</summary>
    public virtual void PreLoad() { }

    /// <summary>Called at the end of <see cref="Load()"/>.</summary>
    public virtual void PostLoad() { }

    /// <summary>Called upon loading a single item.</summary>
    public abstract void Load(Mod mod, TypeInfo type);

    public Func<TypeInfo, bool> LoadCondition { get; }
}