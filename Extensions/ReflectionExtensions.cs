using System;
using System.Collections.Generic;
using System.Reflection;
using WebCom.Reflection;

namespace WebCom.Extensions;

public static class ReflectionExtensions
{
    public static T Create<T>(this Type type)
    {
        return (T) Activator.CreateInstance(type);
    }

    /// <summary>Creates types from the provided types and casts them to <typeparamref name="T"/>.</summary>
    public static IEnumerable<T> Create<T>(this IEnumerable<Type> types)
    {
        foreach (var type in types)
        {
            yield return Create<T>(type);
        }
    }

    /// <summary>Gets all concrete types which are subclasses of <typeparamref name="T"/> from the provided assemblies.</summary>
    public static IEnumerable<Type> Concrete<T>(this IEnumerable<Assembly> assemblies)
    {
        foreach (var assembly in assemblies)
        {
            foreach (var type in Concrete<T>(assembly.GetTypes()))
            {
                yield return type;
            }
        }
    }

    /// <returns>List of all type which are subclasses of <typeparamref name="T"/> and are not abstract or interface.</returns>
    public static IEnumerable<Type> Concrete<T>(this IEnumerable<Type> source)
    {
        foreach (var type in source)
        {
            if (type.IsAbstract || type.IsInterface)
            {
                continue;
            }

            var superType = typeof(T);
            if (superType.IsAssignableFrom(type))
            {
                yield return type;
            }
        }
    }

    internal static List<MemberInfoWrapper> GetDataMembers(this Type type)
    {
        var fields = type.GetFields();
        var properties = type.GetProperties();

        var members = new List<MemberInfoWrapper>(fields.Length + properties.Length);

        foreach (var field in fields)
        {
            members.Add(new MemberInfoWrapper.Field(field));
        }

        foreach (var property in properties)
        {
            members.Add(new MemberInfoWrapper.Property(property));
        }

        return members;
    }

    internal static List<MemberInfoWrapper> GetDataMembers(this Type type, BindingFlags bindingFlags) => GetDataMembers(type, bindingFlags, bindingFlags);

    internal static List<MemberInfoWrapper> GetDataMembers(this Type type, BindingFlags fieldFlags, BindingFlags propertyFlags)
    {
        var fields = type.GetFields(fieldFlags);
        var properties = type.GetProperties(propertyFlags);

        var members = new List<MemberInfoWrapper>(fields.Length + properties.Length);

        foreach (var field in fields)
        {
            members.Add(new MemberInfoWrapper.Field(field));
        }

        foreach (var property in properties)
        {
            members.Add(new MemberInfoWrapper.Property(property));
        }

        return members;
    }
}
