using System;
using System.Collections.Generic;
using System.IO;
using Terraria.ModLoader;
using WebCom.Extensions;
using WebCom.Networking.Serialization;

namespace WebCom.Networking;

public class PacketLoader : ModSystem
{
    private readonly Dictionary<Mod, LocalPacketLoader> _packetLoaders = new();
    private readonly PacketSerializers _serializers = new PacketSerializers.DefaultPacketSerializers();

    public override void Load()
    {
        var mods = ModLoader.Mods.GetNonNullAssemblyMods();

        foreach (var mod in mods)
        {
            var localLoader = new LocalPacketLoader(mod, new PacketMapper(_serializers));
            localLoader.Initialize();

            _packetLoaders.Add(mod, localLoader);
        }
    }

    /// <summary>Finds and handles the received packet.</summary>
    public void HandlePacket(Mod mod, BinaryReader reader, int whoAmI)
    {
        _packetLoaders[mod].HandlePacket(reader, whoAmI);
    }

    public Packet GetPacket(Mod mod, Type type) => _packetLoaders[mod].GetPacket(type);
    public Packet PreparePacket(Mod mod, Packet packet) => _packetLoaders[mod].PreparePacket(packet);
}

public static class PacketLoaderExtensions
{
    public static T GetPacket<T>(this Mod mod) where T : Packet => 
        ModContent.GetInstance<PacketLoader>().GetPacket(mod, typeof(T)) as T;

    public static T PreparePacket<T>(this Mod mod, T packet) where T : Packet =>
        ModContent.GetInstance<PacketLoader>().PreparePacket(mod, packet) as T;
}