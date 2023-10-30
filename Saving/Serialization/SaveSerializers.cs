using System;
using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using WebCom.Extensions;
using WebCom.Networking.Serialization;
using WebCom.Proxies;
using WebCom.Reflection;
using WebCom.Serializers;

namespace WebCom.Saving.Serializations;

/// <summary>Provides the methods for a specific datatype to be handled when loaded or saved.</summary>
public class SaveSerializer : Serializer<SaveSerializer.DataReader, SaveSerializer.DataWriter>
{
    public delegate object DataReader(MemberInfoWrapper member, TagCompound data);
    public delegate void DataWriter(object obj, MemberInfoWrapper member, TagCompound data);
}

public abstract class SaveSerializers : Serializers<SaveSerializer>
{
    internal class DynamicSaveSerializers : SaveSerializers
    {
        private SaveSerializer CreateSerializer(Type type)
        {
            if (IsDictionary(type) && !TagSerializer.TryGetSerializer(type, out var serialier))
            {
                var types = type.GetGenericArguments();

                var serializer = MakeDictionary(types[0], types[1]);
                ModContent.GetInstance<TagSerializerProxy>().AddSerializer(serializer);
            }

            var m = typeof(TagCompound).GetMethod(nameof(TagCompound.Get)).MakeGenericMethod(type);
            object ReadData(MemberInfoWrapper member, TagCompound data)
            {
                return m.Invoke(data, new object[] { member.Member.Name });
            }

            return new()
            {
                Reader = ReadData,
                Writer = WriteData
            };
        }

        private void WriteData(object inst, MemberInfoWrapper member, TagCompound data)
        {
            data.Add(member.Member.Name, member.GetValue(inst));
        }

        private static bool IsDictionary(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>);
        }

        private static TagSerializer MakeDictionary(Type key, Type value) => Activator.CreateInstance(typeof(DynamicDictionarySerializer<,,>)
                .MakeGenericType(typeof(Dictionary<,>).MakeGenericType(key, value), key, value)) as TagSerializer;

        public override SaveSerializer Get(Type type)
        {
            return serializers.GetOrDefault(type, CreateSerializer);
        }

        internal class DynamicDictionarySerializer<D, K, V> : TagSerializer<D, TagCompound> where D : Dictionary<K, V>
        {
            public DynamicDictionarySerializer()
            {
                if (typeof(K).IsSubclassOf(typeof(IConvertible)))
                    throw new InvalidCastException($"The key of the dictionary must implement {nameof(IConvertible)} to support auto-saving dictionaries.");
            }

            public override bool IsLoadingEnabled(Mod mod)
            {
                return false;
            }

            public override D Deserialize(TagCompound tag)
            {
                var dictionary = (D) new Dictionary<K, V>(tag.Count);

                foreach (var entry in tag)
                {
                    dictionary.Add((K) Convert.ChangeType(entry.Key, typeof(K)), tag.Get<V>(entry.Key));
                }

                return dictionary;
            }

            public override TagCompound Serialize(D value)
            {
                var tag = new TagCompound();

                foreach ((K k, V v) in value)
                {
                    tag.Add(k.ToString(), v);
                }

                return tag;
            }
        }
    }
}
