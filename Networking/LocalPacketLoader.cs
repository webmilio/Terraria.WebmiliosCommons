using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using WebCom.Extensions;

namespace WebCom.Networking;

/// <summary>Handles loading packet classes for each individual mods.</summary>
internal class LocalPacketLoader
{
	private ushort _typeId = 1;

	private readonly Dictionary<ushort, Type> _types = new();
	private readonly Dictionary<Type, ushort> _typeIds = new();

    internal LocalPacketLoader(Mod mod, PacketMapper mapper)
	{
		Mod = mod;

		Mapper = mapper;
	}

	internal void Initialize()
	{
		foreach (var type in Mod.Code.GetTypes().Concrete<Packet>())
		{
			_types.Add(_typeId, type);
			_typeIds.Add(type, _typeId);

			_typeId++;

			Mapper.Map(type);
		}
	}

	public void HandlePacket(BinaryReader reader, int fromWho)
	{
		var packetId = reader.ReadUInt16();
		var type = _types[packetId];

		var packet = GetPacket(packetId, type);
		packet.Receive(reader, fromWho);
	}

	public Packet GetPacket(Type type) => GetPacket(_typeIds[type], type);
	public Packet PreparePacket(Packet packet)
	{
		var type = packet.GetType();
		return PreparePacket(_typeIds[type], type, packet);
    }

    internal Packet GetPacket(ushort typeId, Type type)
    {
		return PreparePacket(typeId, type, Activator.CreateInstance(type) as Packet);
    }

	internal Packet PreparePacket(ushort typeId, Type type, Packet packet)
	{
        // TODO Temporary fix until I figure out how to only send packets when connected to a server.
		// Mod.GetPacket() crashes when called in singleplayer.
        if (Main.netMode == NetmodeID.SinglePlayer) 
		{
			return packet;
		}

        packet.PacketTypeId = typeId;

        packet.Mod = Mod;
        packet.ModPacket = Mod.GetPacket();

		packet.Mapper = Mapper;
        packet.Mappings = Mapper.Get(type);

        return packet;
    }

    public Mod Mod { get; }

	public PacketMapper Mapper { get; }
}
