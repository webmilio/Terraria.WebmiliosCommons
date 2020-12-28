using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria.ModLoader.IO;

namespace WebmilioCommons.Commons.Saving
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class SaveAttribute : Attribute
    {
        public delegate object TagIOMethod(object inst, object value);

        public delegate bool PreDelegate(TagCompound tag);
        public delegate void ModDelegate(TagCompound tag);


        public SaveAttribute()
        {
        }

        public SaveAttribute(string nbtKey)
        {
            NBTKey = nbtKey;
        }


        public void Save(object inst, TagCompound tag, PropertyInfo property)
        {
            tag.Add(GetNBTKey(property), Serialize(inst, property.GetValue(inst)));
        }

        public void Load(object inst, TagCompound tag, PropertyInfo property)
        {
            var method = typeof(TagCompound).GetMethod(nameof(TagCompound.Get)).MakeGenericMethod(property.PropertyType);
            property.SetValue(inst, Deserialize(inst, method.Invoke(tag, new object[] { GetNBTKey(property) })));
        }


        public static TagCompound SaveObject(object inst, Dictionary<Type, List<(PropertyInfo property, SaveAttribute save)>> types, PreDelegate preMethod, ModDelegate modMethod)
        {
            var tag = new TagCompound();

            if (preMethod != default && !preMethod(tag))
                return tag;

            var type = inst.GetType();

            if (types.ContainsKey(type))
                foreach (var ps in types[type])
                    ps.save.Save(inst, tag, ps.property);

            modMethod?.Invoke(tag);

            return tag;
        }

        public static void LoadObject(object inst, TagCompound tag, Dictionary<Type, List<(PropertyInfo property, SaveAttribute save)>> types, PreDelegate preMethod, ModDelegate modMethod)
        {
            if (preMethod != default && !preMethod(tag))
                return;

            var type = inst.GetType();

            if (types.ContainsKey(type))
                foreach (var ps in types[type])
                    ps.save.Load(inst, tag, ps.property);

            modMethod?.Invoke(tag);
        }


        public string GetNBTKey(PropertyInfo property) => NBTKey ?? property.Name;

        public virtual object Serialize(object inst, object value) => value;
        public virtual object Deserialize(object inst, object value) => value;


        public string NBTKey { get; set; }
    }
}