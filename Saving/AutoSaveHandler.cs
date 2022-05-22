using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using WebmilioCommons.Extensions;
using WebmilioCommons.Players;
using ModPlayer = WebmilioCommons.Players.ModPlayer;

namespace WebmilioCommons.Saving;

public class AutoSaveHandler : ModSystem
{
    private Dictionary<Type, SaveMemberProxy[]> _playerProxies = new();

    public override void Load()
    {
        InitializeSerializers();

        _playerProxies = InitializePlayers();
    }

    public void Save(ModPlayer player, TagCompound tag) => ForPlayer(player, proxy => proxy.Serialize(player, tag));
    public void Load(ModPlayer player, TagCompound tag) => ForPlayer(player, proxy => proxy.Deserialize(player, tag));

    private void ForPlayer(ModPlayer player, Action<SaveMemberProxy> action) => _playerProxies[player.GetType()].Do(action);

    private static Dictionary<Type, SaveMemberProxy[]> InitializePlayers()
    {
        Dictionary<Type, SaveMemberProxy[]> proxies = new();

        ModStore.ForTypes<ModPlayer>(delegate (Mod mod, TypeInfo type)
        {
            var saveProxies = GetProxies(type);
            proxies.Add(type, saveProxies);
        });

        return proxies;
    }

    private static void InitializeSerializers()
    {
        MethodInfo addSerializer = typeof(TagSerializer).GetMethod("AddSerializer", BindingFlags.Static | BindingFlags.NonPublic);

        if (addSerializer == null)
            throw new InvalidOperationException($"Could not find {nameof(TagSerializer)}.AddSerializer method.");

        foreach (var m in ModStore.Mods)
        {
            foreach (var t in m.Code.Concrete())
            {
                if (!t.TryGetCustomAttribute(out SaveAttribute a))
                    continue;

                var serializer = InitializeSerializer(t);
                addSerializer.Invoke(null, new object[] { serializer });
            }
        }
    }

    private static TagSerializer InitializeSerializer(TypeInfo type)
    {
        var proxies = GetProxies(type);
        var genericSerializer = typeof(MembersProxySerializer<>).MakeGenericType(type);
        var ctor = genericSerializer.GetConstructor(new[] { typeof(SaveMemberProxy[]) });

        // ReSharper disable once PossibleNullReferenceException
        return (TagSerializer)ctor.Invoke(new object[] { proxies });
    }

    private static SaveMemberProxy[] GetProxies(TypeInfo type)
    {
        var fields = GetFieldProxies(type);
        var properties = GetPropertyProxies(type);

        List<SaveMemberProxy> proxies = new(fields.Count + properties.Count);

        proxies.AddRange(fields);
        proxies.AddRange(properties);

        return proxies.ToArray();
    }

    private static List<SaveMemberProxy> GetFieldProxies(TypeInfo type) => GetProxies(type.GetFields, SaveMemberProxy.ForField);
    private static List<SaveMemberProxy> GetPropertyProxies(TypeInfo type) => GetProxies(type.GetProperties, SaveMemberProxy.ForProperty);

    private static List<SaveMemberProxy> GetProxies<T>(Func<BindingFlags, T[]> membersProvider, Func<T, SaveAttribute, SaveMemberProxy> proxifier) where T : MemberInfo
    {
        var members = membersProvider(BindingFlags.Instance | BindingFlags.Public);
        List<SaveMemberProxy> proxies = new(members.Length);

        members.Do(delegate (T instance, int index)
        {
            if (!instance.TryGetCustomAttribute<SaveAttribute>(out var attr))
                return;

            proxies.Add(proxifier(instance, attr));
        });

        return proxies;
    }
}