using System;
using System.Reflection;
using Terraria.ModLoader;
using WebmilioCommons.Commons;

namespace WebmilioCommons.Loaders;

public abstract class SingletonPrototypeLoader<TLoader, TLoaderOf> : PrototypeLoader<TLoaderOf>, IUnloadOnModUnload where TLoader : PrototypeLoader<TLoaderOf>, new()
{
    protected static TLoader _instance;

    protected SingletonPrototypeLoader() : this(typeInfo => true) { }

    protected SingletonPrototypeLoader(Func<TypeInfo, bool> loadCondition) : base(loadCondition)
    {
        Load();
    }

    /// <summary>Unique TYPE instance (two same <see cref="TLoader"/> will have the same instance), instantiated and loaded upon first call.</summary>
    public static TLoader Instance => _instance ??= new TLoader();
}