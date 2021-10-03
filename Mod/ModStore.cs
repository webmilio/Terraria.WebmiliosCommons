using System;
using System.Reflection;
using Terraria.ModLoader;
using WebmilioCommons.Extensions;

namespace WebmilioCommons
{
    public class ModStore
    {
        private static Mod[] _mods;

        public static void ForTypes<T>(Action<Mod, TypeInfo> action)
        {
            Mods.Do(m => m.Code.Concrete<T>().Do(t => action(m, t)));
        }

        public static Mod[] Mods => 
            _mods ??= ModLoader.Mods.StandardModFilter().ToArray();
    }
}