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

        public static void RegisterKeybinds<T>(T mod) where T : Mod => RegisterKeybinds(mod, mod);
        public static void RegisterKeybinds(IModType instance) => RegisterKeybinds(instance, instance.Mod);

        public static void RegisterKeybinds(object instance, Mod mod)
        {
            RegisterKeybinds(mod, instance.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => p.PropertyType == typeof(ModKeybind)), (a, m, p) => a.RegisterKeybind(m, instance, p));

            RegisterKeybinds(mod, instance.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public)
                .Where(f => f.FieldType == typeof(ModKeybind)), (a, m, f) => a.RegisterKeybind(m, instance, f));
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

        public void RegisterKeybind(Mod mod, object owner, FieldInfo field) => RegisterKeybind(mod, owner, field.SetValue);
        public void RegisterKeybind(Mod mod, object owner, PropertyInfo property) => RegisterKeybind(mod, owner, property.SetValue);

        public void RegisterKeybind(Mod mod, object owner, Action<object, ModKeybind> setter)
        {
            var keybind = KeybindLoader.RegisterKeybind(mod, Name, DefaultBinding);
            setter(owner, keybind);
        }

        public string Name { get; set; }

        public string DefaultBinding { get; set; }
    }
}