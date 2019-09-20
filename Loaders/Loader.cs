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

        protected Dictionary<Type, ushort> idByType = new Dictionary<Type, ushort>();
        protected Dictionary<ushort, Type> typeById = new Dictionary<ushort, Type>();
        protected Dictionary<Type, Mod> modByType = new Dictionary<Type, Mod>();


        protected Loader() : this(typeInfo => true) { }

        protected Loader(Func<TypeInfo, bool> loadCondition)
        {
            LoadCondition = loadCondition;
        }


        public void Load()
        {
            if (Loaded) return;

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

            idByType.Add(item.GetType(), itemId);
            typeById.Add(itemId, item.GetType());
            modByType.Add(item.GetType(), mod);

            PostAdd(mod, item);

            return item;
        }

        protected virtual void PostAdd(Mod mod, T item) { }


        public T New(ushort id) => New(typeById[id]);
        public T New(Type type) => Activator.CreateInstance(type) as T;
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


        public int Count => idByType.Count;
    }
}