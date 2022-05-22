using System;
using System.Collections.Generic;
using System.Reflection;
using MonoMod.RuntimeDetour;
using Terraria.ModLoader;
using WebmilioCommons.Extensions;
using GlobalTile = WebmilioCommons.Tiles.GlobalTile;

namespace WebmilioCommons.Proxies;

public abstract class Proxy<T> : ModSystem
{
    protected const BindingFlags NormalFieldFlags = BindingFlags.NonPublic | BindingFlags.Static;

    public override void Load()
    {
        try
        {
            Mod.Logger.Info($"Initializing Proxy for type {typeof(T).Name}.");
            Items = GetSource();
        }
        catch
        {
            Mod.Logger.ErrorFormat($"Error while fetching the internal list for {typeof(T).Name}. None of the additional hooks will be triggered.");
            Items = new List<T>();
        }
    }

    public override void Unload()
    {
        Items = null;
    }

    protected abstract IList<T> GetSource();

    /// <summary>Not optimal for actions called often, see <see cref="Do"/></summary>
    /// <typeparam name="V"></typeparam>
    /// <returns>All instances of the specified type.</returns>
    public static List<V> Get<V>()
    {
        List<V> items = new(Items.Count);

        Items.Do(delegate (T entry)
        {
            if (entry is V v)
                items.Add(v);
        });

        return items;
    }

    public static bool All<V>(Predicate<V> predicate) => All(item => item is not V v || predicate(v));
    public static bool All(Predicate<T> predicate)
    {
        for (int i = 0; i < Items.Count; i++)
        {
            if (!predicate(Items[i]))
                return false;
        }

        return true;
    }

    public static bool Any<V>(Predicate<V> predicate) => Any(item => item is V v && predicate(v));
    public static bool Any(Predicate<T> predicate)
    {
        for (int i = 0; i < Items.Count; i++)
        {
            if (predicate(Items[i]))
                return true;
        }

        return false;
    }

    public static void Do(Action<T> action) => Items.Do(action);
    public static void Do<V>(Action<V> action)
    {
        Items.Do(delegate (T entry)
        {
            if (entry is V v)
                action(v);
        });
    }

    public void Add(T item)
    {
        Items.Add(item);
    }

    public bool Remove(T item)
    {
        return Items.Remove(item);
    }

    protected static IList<T> Items { get; set; }
}

public abstract class Proxy<TOriginal, TTransformed> : Proxy<TOriginal>
{
    private const BindingFlags HooksBindingFlags = BindingFlags.NonPublic | BindingFlags.Static;

    private MethodInfo _hookMethod, _freeMethod;

    public override void Load()
    {
        base.Load();

        _hookMethod = typeof(TTransformed).GetMethod(nameof(GlobalTile.Hook), HooksBindingFlags);

        if (_hookMethod != null)
            _freeMethod = typeof(TTransformed).GetMethod(nameof(GlobalTile.Free), HooksBindingFlags);

        if (_freeMethod == null)
            _hookMethod = null;

        _hookMethod?.Invoke(null, null);
    }

    public override void Unload()
    {
        base.Unload();

        _freeMethod?.Invoke(null, null);
    }
}