using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria.ModLoader;
using WebmilioCommons.Extensions;

namespace WebmilioCommons.Helpers;

public abstract class ProxyHelper<TGlobal, TBound> : ModSystem
{
    public override void Load()
    {
        try
        {
            Items = GetSource();
        }
        catch
        {
            Mod.Logger.ErrorFormat($"Error while fetching the internal list for {typeof(TGlobal).Name}. None of the additional player hooks will be triggered.");
            Items = new List<TGlobal>();
        }
    }

    protected abstract IList<TGlobal> GetSource();

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

        Items.Do(delegate (TGlobal player)
        {
            if (player is V v)
                items.Add(v);
        });

        return items;
    }

    public static bool All<V>(Predicate<V> predicate) => All(item => item is not V v || predicate(v));
    public static bool All(Predicate<TGlobal> predicate)
    {
        for (int i = 0; i < Items.Count; i++)
        {
            if (!predicate(Items[i]))
                return false;
        }

        return true;
    }

    public static bool Any<V>(Predicate<V> predicate) => Any(item => item is V v && predicate(v));
    public static bool Any(Predicate<TGlobal> predicate)
    {
        for (int i = 0; i < Items.Count; i++)
        {
            if (predicate(Items[i]))
                return true;
        }

        return false;
    }

    public static void Do(Action<TGlobal> action) => Items.Do(action);
    public static void Do<V>(Action<V> action)
    {
        Items.Do(delegate (TGlobal player)
        {
            if (player is V v)
                action(v);
        });
    }

    public void Add(TGlobal item)
    {
        Items.Add(item);
    }

    public bool Remove(TGlobal item)
    {
        return Items.Remove(item);
    }

    protected static IList<TGlobal> Items { get; set; }
}