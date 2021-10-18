using System.Collections.Generic;
using System.Reflection;
using Terraria.ModLoader;

namespace WebmilioCommons.Extensions
{
    public static class AssemblyExtensions
    {
        public static Mod GetModFromAssembly(this Assembly assembly) => ModLoader.GetMod(assembly.GetName().Name);

        public static IEnumerable<TypeInfo> Concrete(this Assembly assembly) => assembly.DefinedTypes.Concrete();
        public static IEnumerable<TypeInfo> Concrete<T>(this Assembly assembly) => assembly.DefinedTypes.Concrete<T>();
    }
}
