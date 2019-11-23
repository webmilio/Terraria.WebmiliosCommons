using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria.ModLoader;

namespace WebmilioCommons.Loaders
{
    public abstract class Loader<T> where T : class
    {
        private ushort _latestTypeId = 1;

        protected Dictionary<Type, ushort> idByType;
        protected Dictionary<ushort, Type> typeById;
        protected Dictionary<Type, Mod> modByType;

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
        }


        [Obsolete("Use " + nameof(TryLoad) + ".")]
        public void Load() => TryLoad();

        /// <summary>Tries loading the current loader instance if it has not already been loaded.</summary>
        public void TryLoad()
        {
            if (Loaded) return;

            PreLoad();

            idByType = new Dictionary<Type, ushort>();
            typeById = new Dictionary<ushort, Type>();
            modByType = new Dictionary<Type, Mod>();

            foreach (Mod mod in ModLoader.Mods)
            {
                if (mod.Code == null)
                    continue;

                foreach (TypeInfo type in mod.Code.DefinedTypes.Where(t => !t.IsAbstract && t.IsSubclassOf(typeof(T))).Where(LoadCondition))
                {
                    T item = Activator.CreateInstance(type) as T;
                    Add(mod, item);
                }
            }

            WebmilioCommonsMod.Instance.Logger.Info($"Loaded {Count} different {typeof(T).Name}");
            Loaded = true;

            PostLoad();
        }

        /// <summary>Called directly after the initialization checks during <see cref="TryLoad"/>.</summary>
        public virtual void PreLoad() { }

        /// <summary>Called at the end of <see cref="TryLoad"/>.</summary>
        public virtual void PostLoad() { }


        public virtual void Unload()
        {
            _latestTypeId = 1;

            idByType.Clear();
            typeById.Clear();
            modByType.Clear();
        }


        protected T Add(Mod mod, T item)
        {
            ushort itemId = _latestTypeId++;
            Type type = item.GetType();

            idByType.Add(type, itemId);
            typeById.Add(itemId, type);
            modByType.Add(type, mod);

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


        public T New(ushort id) => New(typeById[id]);

        public T New(Type type)
        {
            T item = Activator.CreateInstance(type) as T;

            if (item is IAssociatedToMod atm)
                atm.Mod = GetMod(type);

            return item;
        }


        public T New<TType>() where TType : class, T => New(typeof(TType)) as TType;


        public Mod GetMod(ushort id) => modByType[typeById[id]];
        public Mod GetMod(T item) => GetMod(item.GetType());
        public Mod GetMod(Type type) => modByType[type];
        public Mod GetMod<TType>() => GetMod(typeof(TType));


        public ushort GetId(T item) => GetId(item.GetType());
        public ushort GetId(Type type) => idByType[type];
        public ushort GetId<TType>() where TType : T => GetId(typeof(TType));


        public Func<TypeInfo, bool> LoadCondition { get; }
        public bool Loaded { get; private set; }


        /// <summary>How many subtypes of <see cref="T"/> have been loaded.</summary>
        public int Count => idByType.Count;
    }
}