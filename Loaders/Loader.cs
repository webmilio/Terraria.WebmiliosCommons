using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria.ModLoader;
using WebmilioCommons.Commons;
using WebmilioCommons.Extensions;

namespace WebmilioCommons.Loaders
{
    public class Loader<T>
    {
        public const int FIRST_INDEX = 1;

        protected Dictionary<Type, int> idByType = new();
        protected Dictionary<int, Type> typeById = new();
        protected Dictionary<Type, Mod> modByType = new();
        protected Dictionary<Type, T> genericByType = new();
        protected Dictionary<string, Type> typeByName = new(StringComparer.OrdinalIgnoreCase);


        /// <summary>Instantiates a new <see cref="Loader{T}"/> and loads all found subtypes of <see cref="T"/> that are not abstract.</summary>
        public Loader() : this(typeInfo => true) { }

        /// <summary>
        /// Instantiates a new <see cref="Loader{T}"/> and loads all found subtypes of <see cref="T"/> that are not abstract and
        /// match the specified <see cref="loadCondition" />load condition(s)</param>.
        /// </summary>
        /// <param name="loadCondition">The condition under which a subclass of <see cref="T"/> should be loaded.</param>
        public Loader(Func<TypeInfo, bool> loadCondition)
        {
            LoadCondition = loadCondition;
            TypeHasUnlocalizedName = typeof(IIdentifiable<string>).IsAssignableFrom(typeof(T));

            NextIndex = FirstIndex;
        }


        [Obsolete("Use " + nameof(TryLoad) + ".")]
        public void Load() => TryLoad();

        /// <summary>Tries loading the current loader instance if it has not already been loaded.</summary>
        public void TryLoad()
        {
            if (Loading || Loaded) 
                return;

            Loading = true;
            PreLoad();

            foreach (Mod mod in ModStore.Mods)
            {
                foreach (TypeInfo type in mod.Code.Concrete<T>().Where(LoadCondition))
                {
                    if (type.TryGetCustomAttribute(out SkipAttribute _))
                        continue;

                    Add(mod, type);
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

            NextIndex = FirstIndex;

            idByType.Clear();
            typeById.Clear();
            modByType.Clear();
            genericByType.Clear();

            if (TypeHasUnlocalizedName)
                typeByName.Clear();

            PostUnload();
        }

        /// <summary>Called at the very beginning of the <see cref="Unload"/> method.</summary>
        protected virtual void PreUnload() { }

        /// <summary>Called at the very end of the <see cref="Unload"/> method.</summary>
        protected virtual void PostUnload() { }

        protected void Add(Mod mod, Type type)
        {
            T item = (T)Activator.CreateInstance(type);
            Add(mod, item);
        }

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

            int itemId = NextIndex;
            NextIndex++;

            Type type = item.GetType();

            idByType.Add(type, itemId);
            typeById.Add(itemId, type);
            modByType.Add(type, mod);
            genericByType.Add(type, item);

            var name = TypeHasUnlocalizedName ? (item as IIdentifiable<string>).Identifier : item.GetType().FullName;
            typeByName.Add(name, type);

            if (item is IAssociatedToMod atm)
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


        /// <summary>Creates a new instance of the requested type.</summary>
        /// <param name="id">The auto-assigned Id of the generic instance.</param>
        /// <returns>The newly instantiated type.</returns>
        public T New(int id) => New(typeById[id]);

        /// <summary>Creates a new instance of the requested type.</summary>
        /// <typeparam name="TType">The type of the generic instance to create; must be a child of <typeparamref name="T"/> and a non-abstract <c>class</c>.</typeparam>
        /// <returns>The newly instantiated object.</returns>
        public TType New<TType>() where TType : class, T => New(typeof(TType)) as TType;

        public T New(Type type)
        {
            T item = (T) Activator.CreateInstance(type);

            if (item is IAssociatedToMod atm)
                atm.Mod = GetMod(type);

            return item;
        }

        /// <summary>Creates a new instance of the requested type.</summary>
        /// <param name="unlocalizedName">
        /// The corresponding value for <see cref="IHasUnlocalizedName.UnlocalizedName"/> of the desired type.
        /// <typeparamref name="T"/> must implement <see cref="IHasUnlocalizedName"/>.
        /// </param>
        /// <returns>The newly instantiated object; otherwise <c>default</c>.</returns>
        public T New(string unlocalizedName)
        {
            return New(typeByName[unlocalizedName]);
        }


        [Obsolete("See Get(...)", true)] public T GetGeneric(int id) => Get(id);
        [Obsolete("See Get(...)", true)] public TSub GetGeneric<TSub>() where TSub : T => Get<TSub>();
        [Obsolete("See Get(...)", true)] public T GetGeneric(Type type) => Get(type);
        [Obsolete("See Get(...)", true)] public T GetGeneric(string unlocalizedName) => Get(unlocalizedName);

        public T Get(int id) => genericByType[typeById[id]];
        public TSub Get<TSub>() where TSub : T => (TSub)genericByType[typeof(TSub)];
        public T Get(Type type) => genericByType[type];

        public T Get(string name)
        {
            return Get(typeByName[name]);
        }

        public Mod GetMod(int id) => modByType[typeById[id]];
        public Mod GetMod(T item) => GetMod(item.GetType());
        public Mod GetMod(Type type) => modByType[type];
        public Mod GetMod<TType>() => GetMod(typeof(TType));

        public Mod GetMod(string unlocalizedName)
        {
            if (!TypeHasUnlocalizedName)
                return default;

            return GetMod(typeByName[unlocalizedName]);
        }


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

        /// <summary>
        /// Gets the Id of the generic instance with the provided unlocalized name.
        /// <see cref="TypeHasUnlocalizedName"/> must be true.
        /// </summary>
        /// <param name="unlocalizedName">The unlocalized name of the generic instance.</param>
        /// <returns>The Id of the generic instance if found; otherwise <c>default</c>.</returns>
        public int GetId(string unlocalizedName)
        {
            if (!TypeHasUnlocalizedName)
                return default;

            return GetId(typeByName[unlocalizedName]);
        }


        public bool Has(string unlocalizedName) => TypeHasUnlocalizedName && typeByName.ContainsKey(unlocalizedName);


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

        [Obsolete("Obsolete, use Generics instead.", true)]
        public IEnumerable<T> GenericEnumerable => Generics;

        public IEnumerable<string> UnlocalizedNames => TypeHasUnlocalizedName ? typeByName.Keys : default;


        public void ForAllGeneric(Action<T> action) => ForAllGeneric((type, t) => action(t));

        public void ForAllGeneric(Action<Type, T> action)
        {
            foreach (KeyValuePair<Type, T> kvp in genericByType)
                action(kvp.Key, kvp.Value);
        }


        public Func<TypeInfo, bool> LoadCondition { get; }

        public bool Loading { get; private set; }
        public bool Loaded { get; private set; }
        
        public bool TypeHasUnlocalizedName { get; }

        /// <summary>How many subtypes of <see cref="T"/> have been loaded.</summary>
        public int Count => idByType.Count;

        /// <summary>The first auto-assigned Id given to the generic instances.</summary>
        public int FirstIndex => FIRST_INDEX;

        /// <summary>The next auto-assigned Id for the next added generic instance.</summary>
        public int NextIndex { get; private set; }
    }
}