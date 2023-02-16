using System;
using System.Collections.Generic;

namespace WebCom.Extensions;

internal static class ReflectionExtensions
{
    internal static List<Type> Concrete<T>(this IList<Type> source)
    {
        var types = new List<Type>(source.Count);

        for (int i = 0; i < source.Count; i++)
        {
            if (!source[i].IsAbstract && !source[i].IsInterface && source[i].IsSubclassOf(typeof(T)))
                types.Add(source[i]);
        }

        return types;
    }
}
