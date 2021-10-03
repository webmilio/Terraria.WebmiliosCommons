using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;
using WebmilioCommons.Saving;

namespace WebmilioCommons.Extensions
{
    public static class TypeExtensions
    {
        public static List<MemberProxy> GetFieldsAndProperties(this TypeInfo type) => GetFieldsAndProperties(type, _ => true);
        public static List<MemberProxy> GetFieldsAndProperties(this TypeInfo type, BindingFlags flags) => GetFieldsAndProperties(type, flags, _ => true);
        public static List<MemberProxy> GetFieldsAndProperties(this TypeInfo type, Predicate<MemberInfo> predicate) => GetFieldsAndProperties(type, BindingFlags.Public | BindingFlags.Instance, predicate);

        public static List<MemberProxy> GetFieldsAndProperties(this TypeInfo type, BindingFlags flags, Predicate<MemberInfo> predicate)
        {
            var properties = type.GetProperties(flags);
            var fields = type.GetFields(flags);

            List<MemberProxy> proxies = new();

            properties.Do(p =>
            {
                if (predicate(p))
                    proxies.Add(MemberProxy.ForProperty(p));
            });

            fields.Do(f =>
            {
                if (predicate(f))
                    proxies.Add(MemberProxy.ForField(f));
            });

            return proxies;
        }

        public static string GetPath(this Type type)
        {
            string[] segments = type.Namespace.Split('.');
            return string.Join("/", segments, 1, segments.Length - 1) + '/' + type.Name;
        }

        [Obsolete("Replaced by GetPath()")]
        public static string GetTexturePath(this Type type) => GetPath(type);

        public static string GetRootPath(this Type type) => GetRootPath(type, false);
        public static string GetRootPath(this Type type, bool includeMod)
        {
            string[] segments = type.Namespace.Split('.');
            return string.Join("/", segments, includeMod ? 0 : 1, segments.Length - 1);
        }
        
        [Obsolete("Replaced by GetRootPath()")]
        public static string GetTexturePathRoot(this Type type) => GetRootPath(type);

        /// <summary>Finds the appropriate texture based solely on the type and its associated mod.</summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Asset<Texture2D> GetTexture(this Type type) => GetTexture(type.GetModFromType(), type);

        public static Asset<Texture2D> GetTexture(this Mod mod, Type type) => mod.Assets.Request<Texture2D>(type.GetPath());

        public static Asset<Texture2D> GetTexture(this Type type, Mod mod) => mod.GetTexture(type);

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