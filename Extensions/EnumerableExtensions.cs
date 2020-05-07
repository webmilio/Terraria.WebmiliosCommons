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
        public static string SplitEveryCapital(this string str) => string.Join(" ", Regex.Split(str, @"(?<!^)(?=[A-Z])"));
    }
}