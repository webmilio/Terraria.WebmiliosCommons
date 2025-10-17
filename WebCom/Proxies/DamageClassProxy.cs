using MonoMod.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria.ModLoader;
using WebCom.Extensions;

namespace WebCom.Proxies;

public class DamageClassProxy
{
    internal static string GetDamageName(DamageClass damage)
    {
        return damage.DisplayName?.Key ?? damage.Name;
    }

    public class Loader : ModSystem
    {
        private static List<DamageClass> _damageClasses;
        private static readonly Dictionary<string, DamageClass> _known = new Dictionary<string, DamageClass>(StringComparer.OrdinalIgnoreCase);

        public static DamageClass Get(string damageClassTypeName)
        {
            return _known.GetOrDefault(damageClassTypeName, FindInLoader);
        }

        private static DamageClass FindInLoader(string name)
        {
            return _damageClasses.FirstOrDefault(dc => GetDamageName(dc).Equals(name, StringComparison.OrdinalIgnoreCase)) ?? DamageClass.Default;
        }

        public override void Load()
        {
            _damageClasses = typeof(DamageClassLoader).GetField("DamageClasses", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null) as List<DamageClass>;

            foreach (var property in typeof(DamageClass).GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (property.PropertyType.IsSubclassOf(typeof(DamageClass)))
                {
                    _known.Add(property.Name, property.GetValue(null) as DamageClass);
                }
            }
        }
    }
}