using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace WebmilioCommons.Extensions
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Transforms a given chance dictionary into an array compromised of X of each element.
        ///
        /// Ex.: 
        /// <example>
        /// A dictionary containing 3 rows as follows:
        /// "x": 2,
        /// "y": 3,
        /// "z": 1
        ///
        /// will result in an array as follows:
        /// { "x", "x", "y", "y", "y", "z" }
        /// </example>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="chances"></param>
        /// <returns></returns>
        public static T[] ToChanceArray<T>(this IDictionary<T, int> chances)
        {
            int total = 0;

            foreach (var kvp in chances)
                total += kvp.Value;


            T[] entries = new T[total];
            int i = 0;

            foreach (var kvp in chances)
                for (int j = 0; j < kvp.Value; j++)
                {
                    entries[i] = kvp.Key;
                    i++;
                }


            return entries;
        }

        public static T Find<T>(this T[] array, Predicate<T> predicate)
        {
            for (var i = 0; i < array.Length; i++)
            {
                if (predicate(array[i]))
                {
                    return array[i];
                }
            }

            return default;
        }

        public static TValue AddOrGet<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, out TValue value, Func<TValue> provider)
        {
            if (dictionary.TryGetValue(key, out value))
                return value;
            
            dictionary.Add(key, value = provider());
            return value;
        }

        /// <summary>Executes a provided action on a sequence of elements. If the provided sequence implements <see cref="IList{T}"/>, the iteration is done through a <c>for</c>, otherwise it is done through a <c>foreach</c>.</summary>
        /// <typeparam name="T">The type of <paramref name="source"/>.</typeparam>
        /// <param name="source"></param>
        /// <param name="action">The action to execute on each element of the sequence.</param>
        public static void Do<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source is IList<T> l)
                Do<T>(l, action);
            else
                foreach (T t in source)
                    action(t);
        }

        public static void Do<T>(this IList<T> source, Action<T> action) => Do(source, (e, i) => action(e));

        public static void Do<T>(this IList<T> source, Action<T, int> action)
        {
            for (int i = 0; i < source.Count; i++)
                action(source[i], i);
        }


        public static void DoInverted<T>(this IList<T> source, Action<T> action) => DoInverted(source, (e, i) => action(e));

        public static void DoInverted<T>(this IList<T> source, Action<T, int> action)
        {
            for (int i = source.Count - 1; i >= 0; i--)
                action(source[i], i);
        }


        public static bool ForAll<T>(this IList<T> source, Predicate<T> predicate)
        {
            for (int i = 0; i < source.Count; i++)
                if (!predicate(source[i]))
                    return false;

            return true;
        }


        public static bool ForAllInverted<T>(this IList<T> source, Predicate<T> predicate) => ForAllInverted(source, (e, i) => predicate(e));

        public static bool ForAllInverted<T>(this IList<T> source, Func<T, int, bool> predicate)
        {
            for (int i = source.Count - 1; i >= 0; i--)
                if (!predicate(source[i], i))
                    return false;

            return true;
        }


        public static List<T> NotNull<T>(this IList<object> source)
        {
            List<T> instances = new List<T>(source.Count);

            for (var i = 0; i < source.Count; i++)
                if (source[i] is T t)
                    instances.Add(t);

            return instances;
        }


        public static string GenerateSlashedString(this List<float> values) => GenerateSlashedString(values.ToArray());

        public static string GenerateSlashedString(this float[] values)
        {
            StringBuilder sb = new StringBuilder();


            int matches = 0;

            matches = values.Count(x => x == values[0]);

            if (matches >= values.Length)
                return values[0].ToString();

            for (int i = 0; i < values.Length; i++)
            {
                sb.Append(values[i]);

                if (i + 1 < values.Length)
                    sb.Append('/');
            }

            return sb.ToString();
        }

        // https://stackoverflow.com/questions/36147162/c-sharp-string-split-separate-string-by-uppercase
        public static string SplitEveryCapital(this string str) => String.Join(" ", Regex.Split(str, @"(?<!^)(?=[A-Z])"));
    }
}