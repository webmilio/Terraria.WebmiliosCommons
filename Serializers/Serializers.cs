using System;
using System.Collections.Generic;
using Terraria.ModLoader;
using WebCom.Networking.Serialization;

namespace WebCom.Serializers;

public abstract class Serializers<T>
{
    protected readonly Dictionary<Type, T> serializers = new();

    /// <summary>Replaces a serializer for another. Must be called in your <see cref="Mod.Load"/> method.</summary>
    /// <param name="type">The property type.</param>
    /// <param name="serializer">The serializer (reader/writer) for the type.</param>
    public virtual T Replace(Type type, T serializer)
    {
        serializers.TryGetValue(type, out var old);
        serializers.Remove(type);

        serializers.Add(type, serializer);
        return old;
    }

    /// <summary>Add a serializer for the given type. Must be called in your <see cref="Mod.Load"/> method.</summary>
    /// <typeparam name="K">The type.</typeparam>
    /// <param name="serializer">The serializer (reader/writer) for the type.</param>
    public void Add<K>(T serializer) => Add(typeof(K), serializer);

    /// <summary>Add a serializer for the given type. Must be called in your <see cref="Mod.Load"/> method.</summary>
    /// <param name="type">The type.</param>
    /// <param name="serializer">The serializer (reader/writer) for the type.</param>
    public virtual void Add(Type type, T serializer)
    {
        serializers.Add(type, serializer);
    }

    /// <summary>Check if there is a serializer defined for a type.</summary>
    /// <param name="type">The property type.</param>
    /// <returns><c>true</c> if there is a serializer for the type; otherwise false.</returns>
    public virtual bool Has(Type type)
    {
        return serializers.ContainsKey(type);
    }

    /// <summary>Check if there is a serializer defined for a type.</summary>
    /// <typeparam name="K">The property type.</typeparam>
    /// <returns><c>true</c> if there is a serializer for the type; otherwise false.</returns>
    public bool Has<K>() => Has(typeof(K));

    /// <summary>Fetch a serializer.</summary>
    /// <param name="type">The type.</param>
    /// <returns>The <typeparamref name="T"/> if found; otherwise <c>null</c>.</returns>
    public virtual T Get(Type type)
    {
        return serializers[type];
    }

    /// <summary>Tries to get a serializer with the associated type.</summary>
    /// <returns><c>true</c> if a serializer was found; otherwise <c>false</c>.</returns>
    public bool TryGet(Type type, out T serializer)
    {
        serializer = default;

        if (!Has(type))
        {
            return false;
        }

        serializer = Get(type);
        return true;
    }

    /// <summary>Fetch a serializer.</summary>
    /// <typeparam name="K">The type.</typeparam>
    /// <returns>The <typeparamref name="T"/> if found; otherwise <c>null</c>.</returns>
    public T Get<K>() => Get(typeof(K));
}
