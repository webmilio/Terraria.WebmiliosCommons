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

        protected virtual void Initialize() { }

        public void InitializeIfNot()
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


        public virtual TSub Add<TSub>(TSub item) where TSub : T
        {
            if (byIndex.Contains(item) || byNames.ContainsKey(item.UnlocalizedName)) return (TSub) byNames[item.UnlocalizedName];

            byIndex.Add(item);
            byNames.Add(item.UnlocalizedName, item);
            return item;
        }

        public virtual TSub Add<TSub>() where TSub : T, new() => Add<TSub>(new TSub());

        public virtual bool Remove(T item)
        {
            if (!byIndex.Contains(item)) return false;

            byIndex.Remove(item);
            byNames.Remove(item.UnlocalizedName);
            return true;
        }


        public virtual bool Contains(T item) => byIndex.Contains(item);

        public virtual bool Contains(string unlocalizedName) => byNames.ContainsKey(unlocalizedName);


        public int GetIndex(T item) => byIndex.IndexOf(item);
        public int GetIndex(string unlocalizedName) => GetIndex(byNames[unlocalizedName]);


        public List<T> Where(Predicate<T> predicate)
        {
            List<T> found = new List<T>();

            for (int i = 0; i < Count; i++)
                if (predicate(this[i]))
                    found.Add(this[i]);

            return found;
        }

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

        public bool IsReadOnly => true; // Behaves like a Read-Only dictionary when it comes to the interface.


        public class ManagerEqualityComparer : IEqualityComparer<string>
        {
            public bool Equals(string x, string y) => x.Equals(y, StringComparison.CurrentCultureIgnoreCase);

            public int GetHashCode(string obj) => obj.GetHashCode();
        }
    }
}