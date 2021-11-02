using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria.ModLoader;

namespace WebmilioCommons.DependencyInjection;

internal class ModContentServiceProvider : IServiceProvider
{
    protected readonly Dictionary<Type, object> instances = new();

    public object GetService(Type serviceType)
    {
        if (instances.TryGetValue(serviceType, out var instance))
        {
            return instance;
        }

        var genericGet = typeof(ModContent).GetMethod(nameof(ModContent.GetInstance), BindingFlags.Public | BindingFlags.Static)
            .MakeGenericMethod(serviceType);

        instance = genericGet.Invoke(null, null);
        instances.Add(serviceType, instance);

        return instance;
    }
}