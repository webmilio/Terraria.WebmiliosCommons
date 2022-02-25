﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria.ModLoader;
using WebmilioCommons.Commons;

namespace WebmilioCommons.Loaders;

public class PrototypeLoader<T> : Loader<T>
{
    protected int nextIndex;

    protected Dictionary<Type, int> idByType = new();
    protected Dictionary<int, Type> typeById = new();
    protected Dictionary<Type, Mod> modByType = new();
    protected Dictionary<Type, T> genericByType = new();

    public PrototypeLoader()
    {
    }

    public PrototypeLoader(Func<TypeInfo, bool> loadCondition) : base(loadCondition)
    {
    }

    public virtual void Unload()
    {
        PreUnload();

        nextIndex = 0;

        idByType.Clear();
        typeById.Clear();
        modByType.Clear();
        genericByType.Clear();

        PostUnload();
    }

    public override void Load(Mod mod, TypeInfo type)
    {
        T item = (T)Activator.CreateInstance(type);
        Add(mod, item);
    }
    
    public override void PostLoad() { }

    /// <summary>Called at the very beginning of the <see cref="Unload"/> method.</summary>
    protected virtual void PreUnload() { }

    /// <summary>Called at the very end of the <see cref="Unload"/> method.</summary>
    protected virtual void PostUnload() { }

    /// <summary>
    /// Add an generic instance to the loaded items.
    /// If <typeparamref name="T"/> implements <see cref="IHasUnlocalizedName"/>, it will also register it using <see cref="IHasUnlocalizedName.UnlocalizedName"/>.
    /// </summary>
    /// <param name="mod">The <see cref="Mod"/> from which the item comes.</param>
    /// <param name="item">A generic instance.</param>
    /// <returns></returns>
    protected T Add(Mod mod, T item)
    {
        if (!PreAdd(mod, item))
            return default;

        Type type = item.GetType();

        idByType.Add(type, nextIndex);
        typeById.Add(nextIndex++, type);
        modByType.Add(type, mod);
        genericByType.Add(type, item);

        if (item is IModLinked atm)
            atm.Mod = mod;

        PostAdd(mod, item, type);

        return item;
    }


    protected virtual bool PreAdd(Mod mod, T item) => true;

    /// <summary>Called after each time a subclass is added to the generic instances database.</summary>
    /// <param name="mod">The mod from which the generic instance originates.</param>
    /// <param name="item">A generic instances created via <see cref="Activator.CreateInstance(System.Type)"/>.</param>
    [Obsolete("Use PostAdd(Mod mod, T item, Type type).", true)]
    protected virtual void PostAdd(Mod mod, T item) { }

    /// <summary>Called after each time a subclass is added to the generic instances database.</summary>
    /// <param name="mod">The <see cref="Mod"/> from which the generic instance originates.</param>
    /// <param name="item">A generic instance.</param>
    /// <param name="type">The <see cref="Type"/> from which the generic instance was created.</param>
    protected virtual void PostAdd(Mod mod, T item, Type type) { }


    public TSub New<TSub>(TSub prototype) where TSub : T
    {
        TSub item = (TSub)Activator.CreateInstance(typeof(TSub));

        if (item is IModLinked atm)
            atm.Mod = ((IModLinked) prototype).Mod;

        return item;
    }

    /// <summary>Creates a new instance of the requested type.</summary>
    /// <param name="id">The auto-assigned Id of the generic instance.</param>
    /// <returns>The newly instantiated type.</returns>
    public T New(int id) => New(typeById[id]);

    /// <summary>Creates a new instance of the requested type.</summary>
    /// <typeparam name="TSub">The type of the generic instance to create; must be a child of <typeparamref name="T"/> and a non-abstract <c>class</c>.</typeparam>
    /// <returns>The newly instantiated object.</returns>
    public TSub New<TSub>() where TSub : class, T => New(typeof(TSub)) as TSub;

    public T New(Type type)
    {
        T item = (T)Activator.CreateInstance(type);

        if (item is IModLinked atm)
            atm.Mod = GetMod(type);

        return item;
    }

    public T Get(int id) => genericByType[typeById[id]];
    public TSub Get<TSub>() where TSub : T => (TSub)genericByType[typeof(TSub)];
    public T Get(Type type) => genericByType[type];

    public Mod GetMod(int id) => modByType[typeById[id]];
    public Mod GetMod(T item) => GetMod(item.GetType());
    public Mod GetMod(Type type) => modByType[type];
    public Mod GetMod<TType>() => GetMod(typeof(TType));

    /// <summary>Gets the Id of the generic instance of the same type as the one provided.</summary>
    /// <param name="item">The type of the generic instance.</param>
    /// <returns>The Id of the generic instance.</returns>
    public int GetId(T item) => GetId(item.GetType());

    /// <summary>Gets the Id of the generic instance of the same type as the one provided.</summary>
    /// <param name="type">The type of the generic instance.</param>
    /// <returns>The Id of the generic instance.</returns>
    public int GetId(Type type) => idByType[type];

    /// <summary>Gets the Id of the generic instance of the same type as the one provided.</summary>
    /// <typeparam name="TType">The type of the generic instance.</typeparam>
    /// <returns>The Id of the generic instance.</returns>
    public int GetId<TType>() where TType : T => GetId(typeof(TType));

    /// <summary>Searches for a generic instance that matches the conditions defined by the specified predicate.</summary>
    /// <param name="condition">The <see cref="Predicate{T}"/> delegate that defines the conditions of the generic instance to search for.</param>
    /// <returns>The generic instance that matches the conditions defined by the specified predicate, if found; otherwise, the default value for type T.</returns>
    public T FindGeneric(Predicate<T> condition)
    {
        foreach (KeyValuePair<Type, T> kvp in genericByType)
            if (condition(kvp.Value))
                return kvp.Value;

        return default;
    }

    /// <summary>An <see cref="IEnumerable{T}"/> that contains all instances created during load (generics).</summary>
    public IEnumerable<T> Generics => genericByType.Values.AsEnumerable();

    public void ForAllGeneric(Action<T> action) => ForAllGeneric((type, t) => action(t));

    public void ForAllGeneric(Action<Type, T> action)
    {
        foreach (KeyValuePair<Type, T> kvp in genericByType)
            action(kvp.Key, kvp.Value);
    }

    /// <summary>How many subtypes of <see cref="T"/> have been loaded.</summary>
    public int Count => idByType.Count;
}