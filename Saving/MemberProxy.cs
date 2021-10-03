using System;
using System.Reflection;

namespace WebmilioCommons.Saving
{
    public class MemberProxy
    {
        public MemberProxy(MemberInfo member, Type type, Action<object, object> set, Func<object, object> get)
        {
            Member = member;
            Type = type;

            Set = set;
            Get = get;
        }

        public static MemberProxy ForField(FieldInfo field)
        {
            return new MemberProxy(field, field.FieldType,
                field.SetValue, field.GetValue);
        }

        public static MemberProxy ForProperty(PropertyInfo property)
        {
            return new MemberProxy(property, property.PropertyType,
                property.SetValue, property.GetValue);
        }

        public MemberInfo Member { get; }
        public Type Type { get; }

        public Action<object, object> Set { get; }
        public Func<object, object> Get { get; }
    }
}