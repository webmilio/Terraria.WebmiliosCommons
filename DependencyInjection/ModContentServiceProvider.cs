using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria.ModLoader;

namespace WebCom.DependencyInjection;

internal class ModContentServiceProvider : IServiceProvider
{
    private readonly MethodInfo _method;
    protected readonly Dictionary<Type, object> instances = new();

    public ModContentServiceProvider()
    {
        _method = typeof(ModContent).GetMethod(nameof(ModContent.GetInstance),
            BindingFlags.Public | BindingFlags.Static);
    }

    public object GetService(Type serviceType)
    {
        if (instances.TryGetValue(serviceType, out var instance))
        {
            return instance;
        }

        var genericGet = _method.MakeGenericMethod(serviceType);

        instance = genericGet.Invoke(null, null);
        instances.Add(serviceType, instance);

        return instance;
    }
}