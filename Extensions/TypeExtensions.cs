using System;
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

        public static string GetRootPath(this Type type)
        {
            string[] segments = type.Namespace.Split('.');
            return string.Join("/", segments, 1, segments.Length - 1);
        }

        [Obsolete("Replaced by GetRootPath()")]
        public static string GetTexturePathRoot(this Type type) => GetRootPath(type);

        /// <summary>Finds the appropriate texture based solely on the type and its associated mod.</summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Texture2D GetTexture(this Type type) => type.GetModFromType().GetTexture(type.GetTexturePath());

        public static Texture2D GetTexture(this Mod mod, Type type) => mod.GetTexture(type.GetTexturePath());

        public static Texture2D GetTexture(this Type type, Mod mod) => mod.GetTexture(type);

        public static Mod GetModFromType(this Type type) => ModLoader.GetMod(type.Namespace.Split('.')[0]);
    }
}