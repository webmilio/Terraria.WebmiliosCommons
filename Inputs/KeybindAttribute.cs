using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Xna.Framework.Input;
using Terraria.ModLoader;
using WebmilioCommons.Extensions;

namespace WebmilioCommons.Inputs
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class KeybindAttribute : Attribute
    {
        public KeybindAttribute(string name, Keys defaultBinding) : this(name, defaultBinding.ToString())
        {
        }

        public KeybindAttribute(string name, string defaultBinding)
        {
            Name = name;
            DefaultBinding = defaultBinding;
        }

        public static void RegisterKeybinds<T>(T mod) where T : Mod
        {
            RegisterKeybinds(mod, typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => p.PropertyType == typeof(ModKeybind)), (a, m, p) => a.RegisterKeybind(m, p));

            RegisterKeybinds(mod, typeof(T).GetFields(BindingFlags.Instance | BindingFlags.Public)
                .Where(f => f.FieldType == typeof(ModKeybind)), (a, m, f) => a.RegisterKeybind(m, f));
        }

        private static void RegisterKeybinds<T>(Mod mod, IEnumerable<T> members, Action<KeybindAttribute, Mod, T> registration) where T : MemberInfo
        {
            foreach (T member in members)
            {
                if (!member.TryGetCustomAttribute<KeybindAttribute>(out var attribute))
                    continue;

                registration(attribute, mod, member);
            }
        }

        public void RegisterKeybind(Mod mod, FieldInfo field) => RegisterKeybind(mod, field.SetValue);
        public void RegisterKeybind(Mod mod, PropertyInfo property) => RegisterKeybind(mod, property.SetValue);

        public void RegisterKeybind(Mod mod, Action<Mod, ModKeybind> setter)
        {
            var keybind = KeybindLoader.RegisterKeybind(mod, Name, DefaultBinding);
            setter(mod, keybind);
        }

        public string Name { get; set; }

        public string DefaultBinding { get; set; }
    }
}