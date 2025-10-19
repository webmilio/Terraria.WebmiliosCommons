using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria.ModLoader.Core;
using WebCom.Reflection;

namespace WebCom.Extensions;

public static class ReflectionExtensions
{
    /// <summary>Translates the full type name (namespace.name) to a path while excluding the first level namespace.</summary>
    public static string GetFullNameAsPath(this Type type)
    {
        var splitNamespace = type.Namespace.Split('.');
        return $"{string.Join('/', splitNamespace, 1, splitNamespace.Length - 1)}/{type.Name}";
    }

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
            foreach (var type in Concrete<T>(AssemblyManager.GetLoadableTypes(assembly)))
            {
                yield return type;
            }
        }
    }

    /// <returns>List of all type which are subclasses of <typeparamref name="T"/> and are not abstract or interface.</returns>
    public static IEnumerable<Type> Concrete<T>(this IEnumerable<Type> source)
    {
        return Concrete(source, typeof(T));
    }

    /// <returns>List of all type which are subclasses of <paramref name="superType"/> and are not abstract or interface.</returns>
    public static IEnumerable<Type> Concrete(this IEnumerable<Type> source, Type superType)
    {
        foreach (var type in source)
        {
            if (type.IsAbstract || type.IsInterface)
            {
                continue;
            }

            if (superType.IsAssignableFrom(type))
            {
                yield return type;
            }
        }
    }

    public static List<MemberInfoWrapper> GetDataMembers(this Type type)
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

    public static List<MemberInfoWrapper> GetDataMembers(this Type type, BindingFlags bindingFlags) => GetDataMembers(type, bindingFlags, bindingFlags);

    public static List<MemberInfoWrapper> GetDataMembers(this Type type, BindingFlags fieldFlags, BindingFlags propertyFlags)
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
