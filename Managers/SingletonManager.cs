using System;
using System.Collections.Generic;

namespace WebmilioCommons.Managers
{
    public abstract class SingletonManager<TManager, TManagerOf> : Manager<TManagerOf> where TManager : Manager<TManagerOf>, new() where TManagerOf : IHasUnlocalizedName
    {
        protected static TManager instance;


        public override void Unload()
        {
            base.Unload();
            instance = null;
        }


        public static TManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new TManager();

                if (!instance.Initialized)
                    instance.DefaultInitialize();

                return instance;
            }
        }
    }
}