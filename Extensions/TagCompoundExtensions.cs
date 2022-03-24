using System;
using System.Collections.Generic;
using Terraria.ModLoader.IO;

namespace WebmilioCommons.Extensions
{
    public static class TagCompoundExtensions
    {
        private const string
            KeysKey = "Keys",
            ValuesKey = "Values";

        public static void AddDictionary<K, V>(this TagCompound tag, string key, Dictionary<K, V> dictionary)
        {
            List<string> keys = new(dictionary.Count);
            List<V> values = new(dictionary.Count);

            foreach (var pair in dictionary)
            {
                keys.Add(pair.Key.ToString());
                values.Add(pair.Value);
            }

            TagCompound container = new()
            {
                { KeysKey, keys },
                { ValuesKey, values }
            };

            tag.Add(key, container);
        }

        public static TagCompound AddFluid(this TagCompound tag, string key, object value)
        {
            tag.Add(key, value);
            return tag;
        }

        public static TagCompound AddFluid(this TagCompound tag, (string key, object value) data)
        {
            tag.Add(data.key, data.value);
            return tag;
        }

        /// <summary></summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="tag"></param>
        /// <param name="key"></param>
        /// <param name="keySelector">Throw a <see cref="ArgumentOutOfRangeException"/> when the value passed to the selector doesn't result in any valid key.</param>
        /// <returns></returns>
        public static Dictionary<K, V> GetDictionary<K, V>(this TagCompound tag, string key, Func<string, K> keySelector)
        {
            var container = tag.Get<TagCompound>(key);
            var keys = container.GetList<string>(KeysKey);
            var values = container.GetList<V>(ValuesKey);
            
            Dictionary<K, V> dictionary = new(keys.Count);

            for (int i = 0; i < keys.Count; i++)
            {
                K k;

                try
                {
                    k = keySelector(keys[i]);
                }
                catch (ArgumentOutOfRangeException)
                {
                    continue; // Invalid value.
                }

                dictionary.Add(k, values[i]);
            }

            return dictionary;
        }
    }
}