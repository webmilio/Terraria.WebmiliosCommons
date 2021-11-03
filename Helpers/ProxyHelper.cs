using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria.ModLoader;
using WebmilioCommons.Extensions;

namespace WebmilioCommons.Helpers;

public abstract class ProxyHelper<T> : ModSystem
{
    public override void Load()
    {
        try
        {
            Items = GetOriginal();
        }
        catch
        {
            Mod.Logger.ErrorFormat($"Error while fetching the internal list for {typeof(T).Name}. None of the additional player hooks will be triggered.");
            Items = Array.Empty<T>();
        }
    }

    protected abstract IList<T> GetOriginal();

    public override void Unload()
    {
        Items = null;
    }

    /// <summary>Not optimal for actions called often, see <see cref="Do"/></summary>
    /// <typeparam name="V"></typeparam>
    /// <returns>All instances of the specified type.</returns>
    public static List<V> Get<V>()
    {
        List<V> items = new(Items.Count);

        Items.Do(delegate (T player)
        {
            if (player is V v)
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
        Items.Do(delegate (T player)
        {
            if (player is V v)
                action(v);
        });
    }

    protected static IList<T> Items { get; set; }
}