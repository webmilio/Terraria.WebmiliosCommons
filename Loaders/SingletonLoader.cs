using System;
using System.Reflection;
using WebmilioCommons.Commons;

namespace WebmilioCommons.Loaders
{
    public abstract class SingletonLoader<TLoader, TLoaderOf> : Loader<TLoaderOf>, IUnloadOnModUnload where TLoader : Loader<TLoaderOf>, new()
    {
        protected static TLoader instance;


        protected SingletonLoader() : this(typeInfo => true) { }

        protected SingletonLoader(Func<TypeInfo, bool> loadCondition) : base(loadCondition)
        {
        }


        [Obsolete("Override PostUnload(). SingletonLoaders are automatically unloaded on mod unload.")]
        public sealed override void Unload()
        {
            base.Unload();

            instance = null;
            WebmilioCommonsMod.Instance.unloadOnModUnload.Remove(this);
        }

        internal override void InternalPostUnload() => WebmilioCommonsMod.Instance.unloadOnModUnload.Add(this);


        /// <summary>Unique TYPE instance (two same <see cref="TLoader"/> will have the same instance), instantiated and loaded upon first call.</summary>
        public static TLoader Instance
        {
            get
            {
                if (instance == null)
                    instance = new TLoader();

                if (!instance.Loaded)
                    instance.TryLoad();

                return instance;
            }
        }
    }
}