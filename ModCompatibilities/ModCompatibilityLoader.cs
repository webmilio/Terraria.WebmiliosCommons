using System;
using System.Collections.Generic;
using Terraria.ModLoader;
using WebmilioCommons.Extensions;
using WebmilioCommons.Loaders;

namespace WebmilioCommons.ModCompatibilities
{
    public sealed class ModCompatibilityLoader : SingletonLoader<ModCompatibilityLoader, ModCompatibility>
    {
        private Dictionary<Mod, List<ModCompatibility>> _compatibilitiesByMods;


        public override void PreLoad()
        {
            _compatibilitiesByMods = new Dictionary<Mod, List<ModCompatibility>>();
        }

        protected override void PostAdd(Mod mod, ModCompatibility item, Type type)
        {
            if (!_compatibilitiesByMods.ContainsKey(mod))
                _compatibilitiesByMods.Add(mod, new List<ModCompatibility>());

            _compatibilitiesByMods[mod].Add(item);
        }

        protected override bool PreAdd(Mod mod, ModCompatibility modCompatibility) => modCompatibility.TryRegister() != default;

        protected override void PostUnload()
        {
            _compatibilitiesByMods?.Clear();
            _compatibilitiesByMods = default;
        }
        

        internal void OnWCLoadFinished() => Generics.Do(modCompat => modCompat.Load());

        internal void OnWCPostSetupContentFinished() => Generics.Do(modCompat => modCompat.PostSetupContent());



        public List<ModCompatibility> GetCompatibilities(Mod mod) => _compatibilitiesByMods[mod];

        public T GetCompatibility<T>() where T : ModCompatibility
        {
            var type = typeof(T);

            if (!genericByType.ContainsKey(type) || !(genericByType[type] is T t))
                return default;

            return t;
        }
    }
}