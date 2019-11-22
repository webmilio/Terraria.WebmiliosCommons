using System;
using System.Reflection;

namespace WebmilioCommons.Loaders
{
    public abstract class SingletonLoader<TLoader, TLoaderOf> : Loader<TLoaderOf> where TLoader : Loader<TLoaderOf>, new() where TLoaderOf : class
    {
        protected static TLoader instance;


        protected SingletonLoader() : this(typeInfo => true) { }

        protected SingletonLoader(Func<TypeInfo, bool> loadCondition) : base(loadCondition)
        {
        }


        public override void Unload()
        {
            base.Unload();

            instance = null;
        }

        
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