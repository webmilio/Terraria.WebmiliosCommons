using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace WebmilioCommons.Extensions
{
    public static class TypeExtensions
    {
        public static string GetPath(this Type type)
        {
            string[] segments = type.Namespace.Split('.');
            return string.Join("/", segments, 1, segments.Length - 1) + '/' + type.Name;
        }

        [Obsolete("Replaced by GetPath()")]
        public static string GetTexturePath(this Type type) => GetPath(type);

        public static string GetRootPath(this Type type, bool includeMod = false)
        {
            string[] segments = type.Namespace.Split('.');
            return string.Join("/", segments, includeMod ? 0 : 1, segments.Length - 1);
        }
        
        [Obsolete("Replaced by GetRootPath()")]
        public static string GetTexturePathRoot(this Type type) => GetRootPath(type);

        /// <summary>Finds the appropriate texture based solely on the type and its associated mod.</summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Texture2D GetTexture(this Type type) => type.GetModFromType().GetTexture(type.GetPath());

        public static Texture2D GetTexture(this Mod mod, Type type) => mod.GetTexture(type.GetPath());

        public static Texture2D GetTexture(this Type type, Mod mod) => mod.GetTexture(type);

        public static Mod GetModFromType(this Type type) => ModLoader.GetMod(type.Namespace.Split('.')[0]);


        public static bool IsTypeFromMod(this Type type, string modName) =>
            type.GetModFromType().Name.Equals(modName, StringComparison.CurrentCultureIgnoreCase);

        public static bool IsTypeFromMod(this object obj, string modName) => IsTypeFromMod(obj.GetType(), modName);


        public static IEnumerable<TypeInfo> Concrete(this IEnumerable<TypeInfo> types) => types.Where(t => !t.IsAbstract && !t.IsInterface);
        public static IEnumerable<TypeInfo> Concrete(this IEnumerable<TypeInfo> types, Type filter) => Concrete(types).Where(filter.IsAssignableFrom);
        public static IEnumerable<TypeInfo> Concrete<T>(this IEnumerable<TypeInfo> types) => Concrete(types, typeof(T));


        public static string NamespaceAsPath(this Type type) => type.Namespace.Replace('.', '\\');

        public static string RootNamespace(this Type type) => type.Namespace.Split('.')[0];
    }
}