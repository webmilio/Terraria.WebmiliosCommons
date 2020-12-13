using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Terraria;

namespace WebmilioCommons.Commons
{
    public sealed class ResourceManager
    {
        public const int DEFAULT_TTL = Constants.TicksPerSecond * 60;

        private static ResourceManager _instance;

        private volatile object _lock = new object();
        private readonly List<IDisposable> _instances = new List<IDisposable>();
        private readonly Dictionary<IDisposable, ManagedResource> _timers = new Dictionary<IDisposable, ManagedResource>();


        private ResourceManager()
        {
            Main.OnTick += MainOnOnTick;
        }

        /// <summary>Allows an object to try to free resources and perform other cleanup operations before it is reclaimed by garbage collection.</summary>
        ~ResourceManager()
        {
            Main.OnTick -= MainOnOnTick;
        }


        /// <summary>Fetches an existing instance of a resource.</summary>
        /// <param name="match">The match filter for the resource.</param>
        /// <returns>The instance matching the filter; otherwise <c>null</c> if not found.</returns>
        public IDisposable Get(Predicate<IDisposable> match)
        {
            IDisposable disposable = _instances.Find(match);

            if (disposable == default)
                return default;

            return disposable;
        }

        /// <summary>Fetches an existing instance of a resource or creates it if it doesn't exist.</summary>
        /// <param name="match">The filter for the resource.</param>
        /// <param name="create">The creation method for the resource.</param>
        /// <param name="ticksToLive">How long the resource should be kept in memory after its last use, in ticks (1 tick = 1/60 of a second).</param>
        /// <returns>The instance matching the filter or the created instance, depending on the <paramref name="create"/> parameter; otherwise <c>null</c> if not found.</returns>
        public IDisposable GetOrCreate(Predicate<IDisposable> match, Func<IDisposable> create, int ticksToLive = DEFAULT_TTL)
        {
            IDisposable disposable = Get(match);

            if (disposable != default)
                return disposable;

            disposable = create();
            Add(disposable, ticksToLive);

            return disposable;
        }


        /// <summary>Fetches an existing instance of a resource.</summary>
        /// <typeparam name="T">The type of resource to find.</typeparam>
        /// <param name="match"></param>
        /// <returns></returns>
        public T Get<T>(Predicate<T> match = default) where T : class, IDisposable
        {
            for (int i = 0; i < _instances.Count; i++)
            {
                T t = _instances[i] as T;

                if (t == null)
                    continue;

                if (match(t) || match == default && t is T)
                    return t;
            }

            return default;
        }

        /// <summary>Fetches an existing instance of a resource or creates it if it doesn't exist.</summary>
        /// <typeparam name="T">The type of resource to look for.</typeparam>
        /// <param name="match">The filter for the resource. If <c>default</c>, will find the first instance of the given type.</param>
        /// <param name="create">The creation method for the resource.</param>
        /// <param name="ticksToLive">How long the resource should be kept in memory after its last use, in ticks (1 tick = 1/60 of a second).</param>
        /// <returns>The instance matching the filter or the created instance, depending on the <paramref name="create"/> parameter; otherwise <c>null</c> if not found.</returns>
        public T GetOrCreate<T>(Predicate<T> match, Func<T> create, int ticksToLive = DEFAULT_TTL) where T : class, IDisposable
        {
            T instance = Get<T>(match);

            if (instance != null)
                return instance;

            instance = create();
            Add(instance, ticksToLive);

            return instance;
        }


        private void Add(IDisposable disposable, int ticksToLive)
        {
            lock (_lock)
            {
                _instances.Add(disposable);
                _timers.Add(disposable, new ManagedResource(ticksToLive, ticksToLive));
            }
        }

        /// <summary>Checks wether the resource should be disposed of next tick.</summary>
        /// <param name="disposable">The resource to verify.</param>
        /// <returns><c>true</c> if the resource should be disposed of; otherwise <c>false</c>.</returns>
        public bool ShouldPrune(IDisposable disposable)
        {
            if (!_instances.Contains(disposable))
                return false;

            return _ShouldPrune(disposable);
        }

        private bool _ShouldPrune(IDisposable disposable) => _timers[disposable].Expired;

        private void Prune(IDisposable disposable)
        {
            lock (_lock)
            {
                _instances.Remove(disposable);
                _timers.Remove(disposable);
            }
        }


        // TODO Verify impact on performance.
        private void MainOnOnTick()
        {
            // Disabled for public release.
            /*lock (_lock)
            {
                bool[] prune = new bool[_instances.Count];
                int index = 0;

                foreach (ManagedResource resource in _timers.Values)
                {
                    resource.Tick();
                    prune[index] = resource.Expired;

                    index++;
                }

                for (int i = prune.Length - 1; i >= 0 ; i--)
                    if (prune[i])
                        Prune(_instances[i]);
            }*/
        }


        public static ResourceManager Instance => _instance ?? (_instance = new ResourceManager());
    }
}
