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


        public static TLoader Instance
        {
            get
            {
                if (instance == null)
                    instance = new TLoader();

                instance.Load();

                return instance;
            }
        }
    }
}