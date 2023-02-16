using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using WebCom.Annotations;
using WebCom.Networking.Serialization;

namespace WebCom.Networking;

public class PacketMapper
{
    private readonly PacketSerializers _serializers;
    private readonly Dictionary<Type, ReadOnlyCollection<MapEntry>> _entries = new();

    internal PacketMapper(PacketSerializers serializers)
    {
        _serializers = serializers;
    }

    public record MapEntry(PropertyInfo Property, PacketSerializer Serializer);

    internal void Map(Type type)
    {
        List<MapEntry> entries = new();

        foreach (var property in type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty | BindingFlags.GetProperty))
        {
            if (property.GetCustomAttribute<SkipAttribute>() != null ||
                !_serializers.TryGet(property.PropertyType, out var serializer))
                continue;

            entries.Add(new(property, serializer));
        } 

        _entries.Add(type, entries.AsReadOnly());
    }

    public ReadOnlyCollection<MapEntry> Get(Type type) => _entries[type];
}
