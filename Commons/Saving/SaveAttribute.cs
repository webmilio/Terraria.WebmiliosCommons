using System;
using System.Reflection;
using Terraria.ModLoader.IO;

namespace WebmilioCommons.Commons.Saving
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class SaveAttribute : Attribute
    {
        public delegate object TagIOMethod(object inst, object value);


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

        public string GetNBTKey(PropertyInfo property) => NBTKey ?? property.Name;

        public virtual object Serialize(object inst, object value) => value;
        public virtual object Deserialize(object inst, object value) => value;


        public string NBTKey { get; set; }
    }
}