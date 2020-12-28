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


        protected BetterModPlayer()
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


        public override TagCompound Save()
        {
            return SaveAttribute.SaveObject(this, _saveTypes, PreSave, ModSave);
        }

        protected virtual bool PreSave(TagCompound tag) => true;

        protected virtual void ModSave(TagCompound tag)
        {
        }


        public override void Load(TagCompound tag)
        {
            SaveAttribute.LoadObject(this, tag, _saveTypes, PreLoad, ModLoad);
        }

        protected virtual bool PreLoad(TagCompound tag) => true;

        protected virtual void ModLoad(TagCompound tag)
        {
        }


        public virtual bool CanInteractWithTownNPCs() => true;
    }
}