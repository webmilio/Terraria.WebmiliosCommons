using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace WebmilioCommons.Extensions
{
    public static class MemberInfoExtensions
    {
        public static bool TryGetCustomAttribute<T>(this MemberInfo member, out T attribute) where T : Attribute
        {
            attribute = member.GetCustomAttribute<T>();
            return attribute != null;
        }

        public static bool TryGetCustomAttributes<T>(this MemberInfo member, out T[] attributes) where T : Attribute
        {
            attributes = member.GetCustomAttributes<T>().ToArray();
            return attributes.Length > 0;
        }
    }
}