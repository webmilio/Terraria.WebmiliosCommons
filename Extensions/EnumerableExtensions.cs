using System;
using System.Collections.Generic;

namespace WebCom.Extensions
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

        /// <summary>Finds the first element matching the <paramref name="predicate"/> and returns it.</summary>
        /// <returns>The first element that matches the <paramref name="predicate"/>; otherwise <c>default</c>.</returns>
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

        /// <summary>Get's a value from the provided dictionary. If the value doesn't exist, it adds it using the <paramref name="provider"/> function.</summary>
        /// <returns>The found or provided value.</returns>
        public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> provider)
        {
            if (dictionary.TryGetValue(key, out var value))
                return value;

            dictionary.Add(key, value = provider(key));
            return value;
        }

        /// <summary>Get's a value from the provided dictionary. If the value doesn't exist, it adds it using the <paramref name="provider"/> function.</summary>
        /// <returns><c>false</c> if the value already existed; <c>true</c> if it was added.</returns>
        public static bool GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, out TValue value, Func<TValue> provider)
        {
            if (dictionary.TryGetValue(key, out value))
                return false;
            
            dictionary.Add(key, value = provider());
            return true;
        }

        /// <summary>
        /// Executes a provided action on a sequence of elements. If the provided sequence implements <see cref="IList{T}"/>, the iteration is done through a <c>for</c>, otherwise it is done through a <c>foreach</c>.
        /// This incurs an overhead cost and is best used in small, rarely-called methods.</summary>
        /// <typeparam name="T">The type of <paramref name="source"/>.</typeparam>
        /// <param name="action">The action to execute on each element of the sequence.</param>
        public static void Do<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source is IList<T> l)
                Do<T>(l, action);
            else
                foreach (T t in source)
                    action(t);
        }

        /// <summary>
        /// Executes a provided action on a sequence of elements.
        /// This incurs an overhead cost and is best used in small, rarely-called methods.</summary>
        /// <typeparam name="T">The type of <paramref name="source"/>.</typeparam>
        /// <param name="action">The action to execute on each element of the sequence.</param>
        public static void Do<T>(this IList<T> source, Action<T> action) => Do(source, (e, i) => action(e));

        /// <summary>
        /// Executes a provided action on a sequence of elements.
        /// This incurs an overhead cost and is best used in small, rarely-called methods.</summary>
        /// <typeparam name="T">The type of <paramref name="source"/>.</typeparam>
        /// <param name="action">The action to execute on each element of the sequence.</param>
        public static void Do<T>(this IList<T> source, Action<T, int> action)
        {
            for (int i = 0; i < source.Count; i++)
                action(source[i], i);
        }

        /// <summary>
        /// Executes a provided action on a sequence of elements, inverted.
        /// This incurs an overhead cost and is best used in small, rarely-called methods.</summary>
        /// <typeparam name="T">The type of <paramref name="source"/>.</typeparam>
        /// <param name="action">The action to execute on each element of the sequence.</param>
        public static void DoInverted<T>(this IList<T> source, Action<T> action) => DoInverted(source, (e, i) => action(e));

        /// <summary>
        /// Executes a provided action on a sequence of elements, inverted.
        /// This incurs an overhead cost and is best used in small, rarely-called methods.</summary>
        /// <typeparam name="T">The type of <paramref name="source"/>.</typeparam>
        /// <param name="action">The action to execute on each element of the sequence.</param>
        public static void DoInverted<T>(this IList<T> source, Action<T, int> action)
        {
            for (int i = source.Count - 1; i >= 0; i--)
                action(source[i], i);
        }

        /// <summary>Determines whether every element in the array of type <typeparamref name="T"/> matches the conditions defined by the specified predicate.</summary>
        public static bool ForAll<T>(this T[] source, Predicate<T> predicate)
        {
            for (int i = 0; i < source.Length; i++)
                if (!predicate(source[i]))
                    return false;

            return true;
        }

        /// <summary>Determines whether every element in the array of type <typeparamref name="T"/> matches the conditions defined by the specified predicate, starting from the end.</summary>
        public static bool ForAllInverted<T>(this IList<T> source, Predicate<T> predicate) => ForAllInverted(source, (e, i) => predicate(e));

        /// <summary>Determines whether every element in the array of type <typeparamref name="T"/> matches the conditions defined by the specified predicate, starting from the end.</summary>
        public static bool ForAllInverted<T>(this IList<T> source, Func<T, int, bool> predicate)
        {
            for (int i = source.Count - 1; i >= 0; i--)
                if (!predicate(source[i], i))
                    return false;

            return true;
        }

        /// <summary>Generates a list of elements by filtering out all null elements in the provided <paramref name="source"/>.</summary>
        public static List<T> NotNull<T>(this IList<object> source)
        {
            var instances = new List<T>(source.Count);

            for (var i = 0; i < source.Count; i++)
                if (source[i] is T t)
                    instances.Add(t);

            return instances;
        }

        /// <summary>Move the element at the specified <paramref name="source"/> index to the specified <paramref name="destination"/> index.</summary>
        public static void Move<T>(this IList<T> list, int source, int destination)
        {
            var item = list[source];
            int direction = source > destination ? -1 : 1;

            for (int i = source; i != destination; i += direction)
                list[i] = list[i + direction];

            list[destination] = item;
        }

        /// <summary>Move the specified <paramref name="element"/> to the specified <paramref name="destination"/>.</summary>
        public static void Move<T>(this IList<T> list, T element, int destination) =>
            Move(list, list.IndexOf(element), destination);
    }
}