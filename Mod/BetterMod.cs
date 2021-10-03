using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria.ModLoader;
using WebmilioCommons.Extensions;

namespace WebmilioCommons
{
    /*public abstract class BetterMod : Mod
    {
        public override void Load()
        {
            RegisterKeybinds(GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => p.PropertyType == typeof(ModKeybind)), (a, m, p) => a.RegisterKeybind(m, p));

            RegisterKeybinds(GetType().GetFields(BindingFlags.Instance | BindingFlags.Public)
                .Where(f => f.FieldType == typeof(ModKeybind)), (a, m, f) => a.RegisterKeybind(m, f));

            ModLoad();
        }

        private void RegisterKeybinds<T>(IEnumerable<T> members, Action<KeybindAttribute, Mod, T> registration) where T : MemberInfo
        {
            foreach (T member in members)
            {
                if (!member.TryGetCustomAttribute<KeybindAttribute>(out var attribute))
                    continue;

                registration(attribute, this, member);
            }
        }

        protected virtual void ModLoad() { }
    }*/
}