using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria.ModLoader;
using WebmilioCommons.Extensions;
using WebmilioCommons.Managers;

namespace WebmilioCommons.Loaders
{
    public abstract class Loader<T>
    {
        public const int FIRST_INDEX = 1;

        protected Dictionary<Type, int> idByType;
        protected Dictionary<int, Type> typeById;
        protected Dictionary<Type, Mod> modByType;
        protected Dictionary<Type, T> genericByType;
        protected Dictionary<string, Type> typeByUnlocalizedName;


        /// <summary>Instantiates a new <see cref="Loader{T}"/> and loads all found subtypes of <see cref="T"/> that are not abstract.</summary>
        protected Loader() : this(typeInfo => true) { }

        /// <summary>
        /// Instantiates a new <see cref="Loader{T}"/> and loads all found subtypes of <see cref="T"/> that are not abstract and
        /// match the specified <param name="loadCondition">load condition(s)</param>.
        /// </summary>
        /// <param name="loadCondition">The condition under which a subclass of <see cref="T"/> should be loaded.</param>
        protected Loader(Func<TypeInfo, bool> loadCondition)
        {
            LoadCondition = loadCondition;
            TypeHasUnlocalizedName = typeof(IHasUnlocalizedName).IsAssignableFrom(typeof(T));
        }


        [Obsolete("Use " + nameof(TryLoad) + ".")]
        public void Load() => TryLoad();

        /// <summary>Tries loading the current loader instance if it has not already been loaded.</summary>
        public void TryLoad()
        {
            if (Loaded) return;

            PreLoad();

            idByType = new Dictionary<Type, int>();
            typeById = new Dictionary<int, Type>();
            modByType = new Dictionary<Type, Mod>();
            genericByType = new Dictionary<Type, T>();

            if (TypeHasUnlocalizedName)
                typeByUnlocalizedName = new Dictionary<string, Type>();

            foreach (Mod mod in ModLoader.Mods)
            {
                if (mod.Code == null)
                    continue;

                foreach (TypeInfo type in mod.Code.Concrete<T>().Where(LoadCondition))
                {
                    T item = (T) Activator.CreateInstance(type);
                    Add(mod, item);
                }
            }

            if (WebmilioCommonsMod.Instance.Logger != default)
                WebmilioCommonsMod.Instance.Logger.Info($"Loaded {Count} different {typeof(T).Name}");
            
            Loaded = true;

            InternalPostUnload();
            PostLoad();
        }

        internal virtual void InternalPostUnload() { }

        /// <summary>Called directly after the initialization checks during <see cref="TryLoad"/>.</summary>
        public virtual void PreLoad() { }

        /// <summary>Called at the end of <see cref="TryLoad"/>.</summary>
        public virtual void PostLoad() { }


        public virtual void Unload()
        {
            PreUnload();

            NextIndex = FIRST_INDEX;

            idByType.Clear();
            typeById.Clear();
            modByType.Clear();
            genericByType.Clear();

            if (TypeHasUnlocalizedName)
                typeByUnlocalizedName.Clear();

            PostUnload();
        }

        protected virtual void PreUnload() { }
        protected virtual void PostUnload() { }


        protected T Add(Mod mod, T item)
        {
            int itemId = NextIndex;
            NextIndex++;

            Type type = item.GetType();

            idByType.Add(type, itemId);
            typeById.Add(itemId, type);
            modByType.Add(type, mod);
            genericByType.Add(type, item);

            if (TypeHasUnlocalizedName)
                typeByUnlocalizedName.Add((item as IHasUnlocalizedName).UnlocalizedName, type);

            if (item is IAssociatedToMod asc)
                asc.Mod = mod;

            PostAdd(mod, item);
            PostAdd(mod, item, type);

            return item;
        }

        /// <summary>Called after each time a subclass is added to the generic instances database.</summary>
        /// <param name="mod">The mod from which the generic instance originates.</param>
        /// <param name="item">A generic instances created via <see cref="Activator.CreateInstance(System.Type)"/>.</param>
        [Obsolete("Use PostAdd(Mod mod, T item, Type type).")]
        protected virtual void PostAdd(Mod mod, T item) { }

        protected virtual void PostAdd(Mod mod, T item, Type type) { }


        public T New(int id) => New(typeById[id]);
        public TType New<TType>() where TType : class, T => New(typeof(TType)) as TType;

        public T New(Type type)
        {
            T item = (T) Activator.CreateInstance(type);

            if (item is IAssociatedToMod atm)
                atm.Mod = GetMod(type);

            return item;
        }

        public T New(string unlocalizedName)
        {
            if (!TypeHasUnlocalizedName)
                return default;

            return New(typeByUnlocalizedName[unlocalizedName]);
        }
        

        public T GetGeneric(int id) => genericByType[typeById[id]];
        public TSub GetGeneric<TSub>() where TSub : T => (TSub)genericByType[typeof(TSub)];
        public T GetGeneric(Type type) => genericByType[type];

        public T GetGeneric(string unlocalizedName)
        {
            if (!TypeHasUnlocalizedName)
                return default;

            return GetGeneric(typeByUnlocalizedName[unlocalizedName]);
        }


        public Mod GetMod(int id) => modByType[typeById[id]];
        public Mod GetMod(T item) => GetMod(item.GetType());
        public Mod GetMod(Type type) => modByType[type];
        public Mod GetMod<TType>() => GetMod(typeof(TType));

        public Mod GetMod(string unlocalizedName)
        {
            if (!TypeHasUnlocalizedName)
                return default;

            return GetMod(typeByUnlocalizedName[unlocalizedName]);
        }


        public int GetId(T item) => GetId(item.GetType());
        public int GetId(Type type) => idByType[type];
        public int GetId<TType>() where TType : T => GetId(typeof(TType));

        public int GetId(string unlocalizedName)
        {
            if (!TypeHasUnlocalizedName)
                return default;

            return GetId(typeByUnlocalizedName[unlocalizedName]);
        }


        public T FindGeneric(Predicate<T> condition)
        {
            foreach (KeyValuePair<Type, T> kvp in genericByType)
                if (condition(kvp.Value))
                    return kvp.Value;

            return default;
        }

        public IEnumerable<T> GenericEnumerable => genericByType.Values.AsEnumerable();

        public IEnumerable<string> UnlocalizedNames => TypeHasUnlocalizedName ? typeByUnlocalizedName.Keys : default;


        public void ForAllGeneric(Action<T> action) => ForAllGeneric((type, t) => action(t));

        public void ForAllGeneric(Action<Type, T> action)
        {
            foreach (KeyValuePair<Type, T> kvp in genericByType)
                action(kvp.Key, kvp.Value);
        }


        public Func<TypeInfo, bool> LoadCondition { get; }
        public bool Loaded { get; private set; }
        public bool TypeHasUnlocalizedName { get; }

        /// <summary>How many subtypes of <see cref="T"/> have been loaded.</summary>
        public int Count => idByType.Count;

        public int FirstIndex => FIRST_INDEX;

        public int NextIndex { get; private set; } = FIRST_INDEX;
    }
}