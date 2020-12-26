using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using WebmilioCommons.Commons.Saving;
using WebmilioCommons.Extensions;

namespace WebmilioCommons.Players
{
    public abstract class BetterModPlayer : ModPlayer
    {
        private static List<Type> _types;
        private static Dictionary<Type, List<PropertyInfo>> _saveTypes;
        private static Dictionary<PropertyInfo, SaveAttribute> _saveProperties;


        internal static void Load()
        {
            _types = new List<Type>();
            _saveTypes = new Dictionary<Type, List<PropertyInfo>>();
            _saveProperties = new Dictionary<PropertyInfo, SaveAttribute>();
        }

        internal static void Unload()
        {
            _types?.Clear();
            _types = default;

            _saveTypes?.Clear();
            _saveTypes = default;

            _saveProperties?.Clear();
            _saveProperties = default;
        }


        protected BetterModPlayer()
        {
            var type = GetType();

            if (_types.Contains(type))
                return;

            _types.Add(type);

            // Save Attribute
            type.GetProperties(BindingFlags.Instance | BindingFlags.Public).Do(property =>
            {
                var saveAttribute = property.GetCustomAttribute<SaveAttribute>();

                if (saveAttribute == default)
                    return;

                if (!_saveTypes.TryGetValue(type, out List<PropertyInfo> properties))
                {
                    properties = new List<PropertyInfo>();
                    _saveTypes.Add(type, properties);
                }

                properties.Add(property);
                _saveProperties.Add(property, saveAttribute);
            });
        }


        public static bool IsInitialized<T>() => _types.Contains(typeof(T));
        public static bool IsInitialized(Type type) => _types.Contains(type);


        public override TagCompound Save()
        {
            var tag = new TagCompound();

            if (!PreSave(tag))
                return tag;

            var type = GetType();

            if (_saveTypes.ContainsKey(type))
                foreach (var property in _saveTypes[type])
                    _saveProperties[property].Save(this, tag, property);

            ModSave(tag);

            return tag;
        }

        protected virtual bool PreSave(TagCompound tag) => true;

        protected virtual void ModSave(TagCompound tag)
        {
        }


        public override void Load(TagCompound tag)
        {
            if (!PreLoad(tag))
                return;

            var type = GetType();

            if (_saveTypes.ContainsKey(type))
                foreach (var property in _saveTypes[type])
                    _saveProperties[property].Load(this, tag, property);

            ModLoad(tag);
        }

        protected virtual bool PreLoad(TagCompound tag) => true;

        protected virtual void ModLoad(TagCompound tag)
        {
        }


        public virtual bool CanInteractWithTownNPCs() => true;
    }
}