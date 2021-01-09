using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria.ModLoader;
using WebmilioCommons.Commons.Saving;
using WebmilioCommons.Extensions;

namespace WebmilioCommons.NPCs
{
    public abstract class BetterModNPC : ModNPC
    {
        private static Dictionary<Type, List<(PropertyInfo property, SaveAttribute save)>> _saveTypes;


        internal static void Load()
        {
            _saveTypes = new Dictionary<Type, List<(PropertyInfo property, SaveAttribute save)>>();
        }

        internal static void Unload()
        {
            _saveTypes?.Clear();
            _saveTypes = default;
        }


        protected BetterModNPC()
        {
            var type = GetType();

            if (_saveTypes.ContainsKey(type))
                return;

            // Save Attribute
            type.GetProperties(BindingFlags.Instance | BindingFlags.Public).Do(property =>
            {
                var saveAttribute = property.GetCustomAttribute<SaveAttribute>();

                if (saveAttribute == default)
                    return;

                if (!_saveTypes.TryGetValue(type, out var properties))
                {
                    properties = new List<(PropertyInfo, SaveAttribute)>();
                    _saveTypes.Add(type, properties);
                }

                properties.Add((property, saveAttribute));
            });
        }


        public static bool IsInitialized<T>() => _saveTypes.ContainsKey(typeof(T));
        public static bool IsInitialized(Type type) => _saveTypes.ContainsKey(type);
    }
}