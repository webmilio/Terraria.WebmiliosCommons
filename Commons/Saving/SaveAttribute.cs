using System;
using System.Reflection;
using Terraria.ModLoader.IO;

namespace WebmilioCommons.Commons.Saving
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class SaveAttribute : Attribute
    {
        public SaveAttribute()
        {
        }

        public SaveAttribute(string nbtKey)
        {
            NBTKey = nbtKey;

        }


        public void Save(TagCompound tag, PropertyInfo property, object inst)
        {
            tag.Add(NBTKey ?? property.Name, property.GetValue(inst));
        }

        public void Load(TagCompound tag, PropertyInfo property, object inst)
        {
            var method = typeof(TagCompound).GetMethod(nameof(TagCompound.Get)).MakeGenericMethod(property.PropertyType);
            property.SetValue(inst, method.Invoke(tag, new object[] { NBTKey ?? property.Name }));
        }


        public string NBTKey { get; set; }
    }
}