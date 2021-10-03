using System;
using System.Reflection;
using Terraria.ModLoader.IO;

namespace WebmilioCommons.Saving
{
    public class SaveMemberProxy : MemberProxy
    {
        public SaveMemberProxy(MemberInfo member, Type type, 
            Action<object, object> set, Func<object, object> get,
            SaveAttribute attribute) : base(member, type, set, get)
        {
            Attribute = attribute;
        }

        public void Serialize(object owner, TagCompound tag)
        {
            Attribute.Serialize(owner, tag, this);
        }

        public void Deserialize(object owner, TagCompound tag)
        {
            Attribute.Deserialize(owner, tag, this);
        }

        public static SaveMemberProxy ForField(FieldInfo field, SaveAttribute attribute)
        {
            return new SaveMemberProxy(field, field.FieldType,
                field.SetValue, field.GetValue,
                attribute);
        }

        public static SaveMemberProxy ForProperty(PropertyInfo property, SaveAttribute attribute)
        {
            return new SaveMemberProxy(property, property.PropertyType,
                property.SetValue, property.GetValue,
                attribute);
        }

        public SaveAttribute Attribute { get; }
    }
}