using System;
using System.Reflection;

namespace WebCom.Reflection;

public abstract class MemberInfoWrapper
{
    public MemberInfoWrapper(MemberInfo member)
    {
        Member = member;
    }

    public abstract void SetValue(object obj, object value);
    public abstract object GetValue(object obj);

    public MemberInfo Member { get; }
    public Type Type { get; init; }

    public class Property : MemberInfoWrapper
    {
        private readonly PropertyInfo _property;

        public Property(PropertyInfo property) : base(property) 
        {
            _property = property;
            Type = property.PropertyType;
        }

        public override void SetValue(object obj, object value)
        {
            _property.SetValue(obj, value);
        }

        public override object GetValue(object instance)
        {
            return _property.GetValue(instance);
        }
    }

    public class Field : MemberInfoWrapper
    {
        private readonly FieldInfo _field;

        public Field(FieldInfo field) : base(field)
        { 
            _field = field;
            Type = field.FieldType;
        }

        public override void SetValue(object obj, object value)
        {
            _field.SetValue(obj, value);
        }

        public override object GetValue(object instance)
        {
            return _field.GetValue(instance);
        }
    }
}
