using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using Terraria.ModLoader.IO;
using WebCom.Annotations;
using WebCom.Extensions;
using WebCom.Reflection;
using WebCom.Saving.Serializations;

namespace WebCom.Saving;

internal class SaveMapper
{
    public record MapEntry(MemberInfoWrapper Member, SaveSerializer Serializer);
    private readonly ReadOnlyCollection<MapEntry> _entries;

    public SaveMapper(Type type, SaveSerializers serializers)
    {
        Serializers = serializers;
        var entries = new List<MapEntry>();

        var members = new List<MemberInfoWrapper>();

        {
            var publicMembers = type.GetDataMembers(BindingFlags.Public | BindingFlags.Instance);
            var privateMembers = type.GetDataMembers(BindingFlags.NonPublic | BindingFlags.Instance);

            members.AddRange(publicMembers);
            members.AddRange(privateMembers);
        }

        foreach (var dataMember in members)
        {
            if (dataMember.Member.TryGetCustomAttributes<SaveAttribute>(out _))
            {
                var serializer = serializers.Get(dataMember.Type);
                entries.Add(new(dataMember, serializer));
            }
        }

        _entries = entries.AsReadOnly();
    }

    public void Save(object obj, TagCompound data)
    {
        foreach (var entry in _entries)
        {
            entry.Serializer.Writer(obj, entry.Member, data);
        }
    }

    public void Load(object obj, TagCompound data)
    {
        foreach (var entry in _entries)
        {
            var value = entry.Serializer.Reader(entry.Member, data);
            entry.Member.SetValue(obj, value);
        }
    }

    public SaveSerializers Serializers { get; }
}
