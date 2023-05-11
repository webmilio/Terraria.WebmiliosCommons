using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using WebCom.Annotations;
using WebCom.Extensions;
using WebCom.Networking.Serialization;

namespace WebCom.Networking;

public class PacketMapper
{
    public record MapEntry(PropertyInfo Property, PacketSerializer Serializer);
    private readonly Dictionary<Type, ReadOnlyCollection<MapEntry>> _entries = new();

    internal PacketMapper(PacketSerializers serializers)
    {
        Serializers = serializers;
    }

    internal void Map(Type type)
    {
        List<MapEntry> entries = new();

        foreach (var property in type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty | BindingFlags.GetProperty))
        {
            var localType = property.PropertyType;

            if (localType.IsArray)
            {
                localType = localType.GetElementType();
            }

            if (property.HasCustomAttribute<SkipAttribute>() || 
                !Serializers.TryGet(localType, out var serializer))
            {
                continue;
            }

            if (property.PropertyType.IsArray)
            {
                var localSerializer = serializer;
                serializer = new PacketSerializer((p, br) => ArrayReader(p, br, localType, localSerializer), (p, v) => ArrayWriter(p, v, localSerializer));
            }

            entries.Add(new(property, serializer));
        } 

        _entries.Add(type, entries.AsReadOnly());
    }

    public static object ArrayReader(Packet packet, BinaryReader reader, Type valueType, PacketSerializer serializer)
    {
        Array array = Array.CreateInstance(valueType, reader.ReadInt32());

        for (int i = 0; i < array.Length; i++)
        {
            array.SetValue(serializer.Reader(packet, reader), i);
        }

        return array;
    }

    public static void ArrayWriter(Packet packet, object value, PacketSerializer serializer)
    {
        var array = (Array)value;
        packet.ModPacket.Write(array.Length);

        for (int i = 0; i < array.Length; i++)
        {
            serializer.Writer(packet, array.GetValue(i));
        }
    }

    public ReadOnlyCollection<MapEntry> Get(Type type) => _entries[type];

    public PacketSerializers Serializers { get; }
}
