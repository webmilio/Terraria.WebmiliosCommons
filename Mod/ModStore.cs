using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria.ModLoader;
using WebmilioCommons.Extensions;

namespace WebmilioCommons
{
    public class ModStore
    {
        private static Mod[] _mods;

        public static List<TypeInfo> OfType<T>()
        {
            List<TypeInfo> types = new();

            ForTypes<T>((m, t) => types.Add(t));
            return types;
        }

        public static void ForTypes<T>(Action<TypeInfo> action)
        {
            ForTypes<T>((_, info) => action(info));
        }

        public static void ForTypes<T>(Action<Mod, TypeInfo> action)
        {
            Mods.Do(m => m.Code.Concrete<T>().Do(t => action(m, t)));
        }

        public static Mod[] Mods => 
            _mods ??= ModLoader.Mods.StandardModFilter().ToArray();
    }
}