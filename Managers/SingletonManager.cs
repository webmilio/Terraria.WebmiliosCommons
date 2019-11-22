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


        /// <summary>Unique TYPE instance (two same <see cref="TManager"/> will have the same instance), instantiated and initialized upon first call.</summary>
        public static TManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new TManager();

                if (!instance.Initialized)
                    instance.TryInitialize();

                return instance;
            }
        }
    }
}