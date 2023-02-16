using System;
using System.Linq;
using System.Reflection;

namespace WebCom.Extensions;

public static class MemberInfoExtensions
{
    /// <summary>Gets a custom attribute of type <typeparamref name="T"/> on the member.</summary>
    /// <returns><c>true</c> if the attribute was found; otherwise <c>false</c>.</returns>
    public static bool TryGetCustomAttribute<T>(this MemberInfo member, out T attribute) where T : Attribute
    {
        attribute = member.GetCustomAttribute<T>();
        return attribute != null;
    }

    /// <summary>Gets custom attributes of type <typeparamref name="T"/> on the member.</summary>
    /// <returns><c>true</c> if attributes were found; otherwise <c>false</c>.</returns>
    public static bool TryGetCustomAttributes<T>(this MemberInfo member, out T[] attributes) where T : Attribute
    {
        attributes = member.GetCustomAttributes<T>().ToArray();
        return attributes.Length > 0;
    }
}