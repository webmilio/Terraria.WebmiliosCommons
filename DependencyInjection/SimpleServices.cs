using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Reflection;
using Terraria;
using WebmilioCommons.Extensions;

#pragma warning disable CS1591

namespace WebmilioCommons.DependencyInjection;

public class SimpleServices : IServiceContainer
{
    protected Dictionary<Type, Type> map = new();
    protected Dictionary<Type, ServiceCreatorCallback> creators = new();
    protected Dictionary<Type, object> instances = new();

    protected List<IServiceProvider> providers = new();
    protected List<IServiceContainer> containers = new();

    public SimpleServices()
    {
        AddService(typeof(SimpleServices), this);
        AddProvider(Main.instance.Services);
    }

    // Getting Services
    public object GetService(Type serviceType)
    {
        object service;

        if (map.TryGetValue(serviceType, out var mappedType))
        {
            if (instances.TryGetValue(mappedType, out service))
            {
                return service;
            }

            service = creators[mappedType](this, mappedType);
            return service;
        }

        service = TryGetFromProviders(serviceType);

        if (service == null)
        {
            throw new ArgumentException($"Cannot find service of type {serviceType.Name}.");
        }

        return service;
    }

    public T GetService<T>()
    {
        return (T) GetService(typeof(T));
    }

    protected object TryGetFromProviders(Type serviceType)
    {
        for (int i = 0; i < providers.Count; i++)
        {
            try
            {
                var service = providers[i].GetService(serviceType);

                if (service != null)
                {
                    return service;
                }
            }
            catch
            {
                // Don't do anything
            }
        }

        return null;
    }

    // Adding Services
    /// <summary>Adds a transient service to the collection.</summary>
    public void AddService(Type serviceType, ServiceCreatorCallback callback) => AddService(serviceType, callback, false);

    /// <summary>Adds a singleton service to the collection.</summary>
    public void AddService(Type serviceType, object serviceInstance) => AddService(serviceType, serviceInstance, false);

    /// <summary>Adds a transient service to the collection.</summary>
    public void AddService(Type serviceType, ServiceCreatorCallback callback, bool promote)
    {
        Map(serviceType);
        creators.Add(serviceType, callback);

        if (promote)
        {
            containers.Do(container => container.AddService(serviceType, callback, true));
        }
    }

    /// <summary>Adds a singleton service to the collection.</summary>
    public void AddService(Type serviceType, object serviceInstance, bool promote)
    {
        Map(serviceType);
        instances.Add(serviceType, serviceInstance);

        if (promote)
        {
            containers.Do(container => container.AddService(serviceType, serviceInstance, true));
        }
    }

    public SimpleServices AddInstance<T>(T instance)
    {
        AddService(typeof(T), instance);
        return this;
    }

    public SimpleServices AddSingleton(Type serviceType)
    {
        AddService(serviceType, (_, type) => Make(type));
        return this;
    }

    public SimpleServices AddSingleton(object instance)
    {
        AddService(instance.GetType(), instance);
        return this;
    }

    public SimpleServices AddSingleton<T>() => AddSingleton(typeof(T));

    public SimpleServices AddSingleton<T>(Func<SimpleServices, T> factory)
    {
        AddService(typeof(T), (_, _) =>
        {
            var service = factory(this);
            instances.Add(typeof(T), service);

            return service;
        });

        return this;
    }

    // Removing Services
    public void RemoveService<T>() => RemoveService(typeof(T));
    public void RemoveService<T>(bool promote) => RemoveService(typeof(T), promote);

    public void RemoveService(Type serviceType) => RemoveService(serviceType, false);

    public void RemoveService(Type serviceType, bool promote)
    {
        var subtypes = GetSubtypes(new(), serviceType);

        subtypes.Do(delegate(Type subtype)
        {
            if (map.TryGetValue(subtype, out var mappedType) && mappedType == serviceType)
            {
                map.Remove(subtype);
            }
        });

        if (instances.ContainsKey(serviceType))
        {
            instances.Remove(serviceType);
        }

        if (promote)
        {
            containers.Do(container => container.RemoveService(serviceType, true));
        }
    }

    // Providers & Containers
    public SimpleServices AddProvider(IServiceProvider provider)
    {
        if (providers.Contains(provider))
        {
            return this;
        }

        providers.Add(provider);
        return this;
    }

    public SimpleServices RemoveProvider(IServiceContainer provider)
    {
        providers.Remove(provider);
        return this;
    }

    public SimpleServices AddContainer(IServiceContainer container)
    {
        if (containers.Contains(container))
        {
            return this;
        }

        containers.Add(container);
        return AddProvider(container);
    }

    public SimpleServices RemoveContainer(IServiceContainer container)
    {
        containers.Remove(container);
        RemoveProvider(container);

        return this;
    }

    public SimpleServices EnableModContentProvider()
    {
        AddProvider(new ModContentServiceProvider());
        return this;
    }

    // Making
    public object Make(Type type)
    {
        var constructor = FindSuitableConstructor(type);

        if (constructor == null)
            throw new TypeInitializationException(type.FullName, new Exception("Could not find a suitable constructor to call."));

        var parameters = constructor.GetParameters();
        object[] services = new object[parameters.Length];

        for (int i = 0; i < parameters.Length; i++)
        {
            services[i] = GetService(parameters[i].ParameterType);
        }

        var service = constructor.Invoke(services);
        instances.Add(type, service);

        return service;
    }

    // Mapping
    public bool HasMapping(Type serviceType)
    {
        return map.ContainsKey(serviceType);
    }

    protected List<Type> GetSubtypes(List<Type> container, Type type)
    {
        container.Add(type);

        foreach (var inter in type.GetInterfaces())
        {
            GetSubtypes(container, inter);
        }

        if (type.BaseType != null && type.BaseType != typeof(object))
        {
            GetSubtypes(container, type.BaseType);
        }

        return container;
    }

    protected void Map(Type serviceType)
    {
        var subtypes = GetSubtypes(new(), serviceType);

        subtypes.Do(delegate(Type subtype)
        {
            if (map.ContainsKey(subtype))
            {
                map[subtype] = serviceType;
            }
            else
            {
                map.Add(subtype, serviceType);
            }
        });
    }

    // Constructor Mapping
    public ConstructorInfo FindSuitableConstructor(Type serviceType)
    {
        ConstructorInfo match = null;
        ParameterInfo[] parameters = Array.Empty<ParameterInfo>();

        foreach (var constructor in serviceType.GetConstructors())
        {
            var @params = constructor.GetParameters();

            if (parameters.Length > @params.Length) continue;

            // Check for mapping for this container, otherwise try to get the service for the others.
            // This ensures that we don't create useless instances in other containers. It is sadly not very efficient since we go through it again if the verification failed.
            if (!VerifyMappings(this, @params) && !VerifyMappings(this, providers, @params)) continue;

            match = constructor;
            parameters = @params;
        }

        return match;
    }

    private bool VerifyMappings(SimpleServices services, IEnumerable<ParameterInfo> parameters)
    {
        foreach (var parameter in parameters)
        {
            if (parameter.IsOptional ||
                !services.HasMapping(parameter.ParameterType))
                return false;
        }

        return true;
    }

    private bool VerifyMappings(SimpleServices owner, IList<IServiceProvider> prov, IEnumerable<ParameterInfo> parameters)
    {
        foreach (var parameter in parameters)
        {
            foreach (var provider in prov)
            {
                if (owner.HasMapping(parameter.ParameterType)) continue;

                if (parameter.IsOptional || 
                    provider is SimpleServices ss && !ss.HasMapping(parameter.ParameterType) || 
                    provider.GetService(parameter.ParameterType) == null)
                    return false;
            }
        }

        return true;
    }

    public static SimpleServices Common { get; } = new();
}