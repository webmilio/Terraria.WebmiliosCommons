using System;
using System.Collections.Generic;
using Terraria;

namespace WebmilioCommons.Managers
{
    public abstract class Manager<T> where T : IHasUnlocalizedName
    {
        protected readonly List<T> byIndex = new List<T>();
        protected readonly Dictionary<string, T> byNames = new Dictionary<string, T>();


        [Obsolete("Override Initialize() instead.")]
        /// <summary>Called when trying to access the <see cref="Manager{T}"/>'s singleton. Since this method is internal, it is imperative to implement the method in every manager. Always call base.DefaultInitialize() at the end.</summary>
        public virtual void DefaultInitialize()
        {
            Initialized = true;
        }

        /// <summary>Called during <see cref="TryInitialize"/>. You would usually store references to definition objects in this method.</summary>
        protected virtual void Initialize() { }

        /// <summary>Tries initializing the current instance <see cref="Manager{T}"/> if it has not been initialized already.</summary>
        public void TryInitialize()
        {
            if (Initialized)
                return;

            DefaultInitialize();
            Initialize();

            Initialized = true;
        }


        public virtual void Unload()
        {
            byIndex.Clear();
            byNames.Clear();
        }

        /// <summary>Tries adding an instance to the manager.</summary>
        /// <typeparam name="TSub"></typeparam>
        /// <param name="item"></param>
        /// <returns>The new instance of an instance of the same <see cref="IHasUnlocalizedName.UnlocalizedName"/> has not been found; the existing instance otherwise.</returns>
        public virtual TSub Add<TSub>(TSub item) where TSub : T
        {
            if (byIndex.Contains(item) || byNames.ContainsKey(item.UnlocalizedName)) return (TSub) byNames[item.UnlocalizedName];

            byIndex.Add(item);
            byNames.Add(item.UnlocalizedName, item);
            return item;
        }

        /// <summary>Tries adding a generic instance (via new <see cref="TSub"/>()) to the manager.</summary>
        /// <typeparam name="TSub"></typeparam>
        /// <returns>The new instance of an instance of the same <see cref="IHasUnlocalizedName.UnlocalizedName"/> has not been found; the existing instance otherwise.</returns>
        public virtual TSub Add<TSub>() where TSub : T, new() => Add<TSub>(new TSub());

        public virtual void AddRange(params T[] items)
        {
            for (int i = 0; i < items.Length; i++)
                Add(items[i]);
        }

        public virtual bool Remove(T item)
        {
            if (!byIndex.Contains(item)) 
                return false;

            byIndex.Remove(item);
            byNames.Remove(item.UnlocalizedName);
            return true;
        }

        public virtual bool Remove(string unlocalizedName)
        {
            if (!byNames.ContainsKey(unlocalizedName))
                return false;

            byIndex.Remove(byNames[unlocalizedName]);
            byNames.Remove(unlocalizedName);
            return true;
        }


        public virtual bool Contains(T item) => byIndex.Contains(item);

        public virtual bool Contains(string unlocalizedName) => byNames.ContainsKey(unlocalizedName);


        [Obsolete("Use IndexOf instead.")]
        public int GetIndex(T item) => IndexOf(item);
        [Obsolete("Use IndexOf instead.")]
        public int GetIndex(string unlocalizedName) => IndexOf(unlocalizedName);


        public int IndexOf(T item) => byIndex.IndexOf(item);
        public int IndexOf(string unlocalizedName) => IndexOf(byNames[unlocalizedName]);


        public T Get<TSub>() where TSub : T
        {
            for (int i = 0; i < Count; i++)
                if (byIndex[i] is TSub)
                    return byIndex[i];

            return default;
        }


        /// <summary>Simple search that returns elements of the manager corresponding to the specified <see cref="Predicate{T}"/>.</summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public List<T> Where(Predicate<T> predicate)
        {
            List<T> found = new List<T>();

            for (int i = 0; i < Count; i++)
                if (predicate(this[i]))
                    found.Add(this[i]);

            return found;
        }

        /// <summary>Returns a random <see cref="T"/> registered in the manager.</summary>
        /// <returns></returns>
        public virtual T GetRandom() => Main.rand.Next(byIndex);

        [Obsolete("Use ForAll.")]
        public void ForAllItems(Action<T> action) => ForAll(action);

        public void ForAll(Action<T> action)
        {
            for (int i = 0; i < byIndex.Count; i++)
                action(byIndex[i]);
        }

        [Obsolete("Use ForAll.")]
        public void ForAllItems(Action<int, T> action) => ForAll(action);

        public void ForAll(Action<int, T> action)
        {
            for (int i = 0; i < byIndex.Count; i++)
                action(i, byIndex[i]);
        }


        /// <summary>Tries obtaining an instance of <see cref="T"/> corresponding to the specified <param name="key">unlocalized name</param>.</summary>
        /// <param name="key">The desired instance's unlocalized name.</param>
        /// <param name="result"></param>
        /// <returns>true if the instance was found; otherwise false.</returns>
        public bool TryGet(string key, out T result)
        {
            if (!byNames.ContainsKey(key))
            {
                result = default;
                return false;
            }

            result = byNames[key];
            return true;
        }


        public IEnumerator<KeyValuePair<string, T>> GetEnumerator() => byNames.GetEnumerator();


        public T this[int index] => byIndex[index];

        public T this[string key] => byNames[key];


        public int Count => byIndex.Count;

        public bool Initialized { get; private set; }

        public ICollection<string> Keys => byNames.Keys;

        public ICollection<T> Values => byNames.Values;


        public class ManagerEqualityComparer : IEqualityComparer<string>
        {
            public bool Equals(string x, string y) => x.Equals(y, StringComparison.CurrentCultureIgnoreCase);

            public int GetHashCode(string obj) => obj.GetHashCode();
        }
    }
}