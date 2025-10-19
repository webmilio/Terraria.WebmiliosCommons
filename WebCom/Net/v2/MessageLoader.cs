using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using WebCom.Extensions;
using WebCom.Net.v2.Serialization;
using WebCom.Serializers;

namespace WebCom.Net.v2;

public class MessageLoader
{
    private static readonly Dictionary<Mod, MessageLoader> _loaders = [];
    private static readonly Serializers<BinarySerializer> _serializers = new BinarySerializers.DefaultPacketSerializers();

    private readonly Mod _mod;
    private readonly Dictionary<int, MessageHandler> _byId = [];
    private readonly Dictionary<Type, int> _byType = [];

    private const int ServerWhoAmI = 255;

    private MessageLoader(Mod mod, Serializers<BinarySerializer> serializers)
    {
        _mod = mod;

        int id = -1;
        var types = mod.Code.GetTypes().Concrete<IMessage>();

        foreach (var type in types)
        {
            var mapped = MapMessage(type, serializers);

            _byId[id] = mapped;
            _byType[type] = id;

            id--;
        }
    }

    internal static MessageHandler MapMessage(Type messageType, Serializers<BinarySerializer> serializers)
    {
        ValidateMessageType(messageType);

        return new MessageHandler(messageType, serializers);
    }

    internal static void ValidateMessageType(Type messageType)
    {
        var ctors = messageType.GetConstructors();
        var paramlessCtor = ctors.FirstOrDefault(ctor => ctor.GetParameters().Length == 0) ??
            throw new NotSupportedException($"Could not find parameterless constructor for type {messageType}.");
    }

    public int GetId(Type messageType)
    {
        return _byType[messageType];
    }

    public void Handle(BinaryReader reader, int whoAmI)
    {
        var type = reader.ReadInt32();

        var message = _byId[type];

    }

    public void Send(IMessage message) => Send(message, -1, -1);

    public void Send(IMessage message, int toClient, int ignoreClient)
    {
        BinaryWriter stream;
        var finish = () => { };

        if (Main.netMode == NetmodeID.SinglePlayer)
        {
            var memory = new MemoryStream(byte.MaxValue);
            stream = new BinaryWriter(memory);

            finish = delegate
            {
                memory.Seek(0, SeekOrigin.Begin);
                using var reader = new BinaryReader(memory);

                Handle(reader, Main.myPlayer);
            };
        }
        else
        {
            var packet = _mod.GetPacket();

            stream = packet;
            finish = () => packet.Send(toClient, ignoreClient);
        }

        var id = _byType[message.GetType()];
        var mapped = _byId[id];

        if (mapped.Send(message, stream))
        {
            finish();
        }

        stream.Dispose();
    }

    public static MessageLoader Get(Mod mod)
    {
        return _loaders.TryGetValue(mod, out var loader) ?
            loader :
            _loaders[mod] = new MessageLoader(mod, _serializers);
    }
}

public static class ModExtensions
{
    public static void SendMessage(this Mod mod, IMessage message) => SendMessage(mod, message, -1, -1);

    public static void SendMessage(this Mod mod, IMessage message, int toClient, int ignoreClient)
    {
        MessageLoader.Get(mod).Send(message, toClient, ignoreClient);
    }
}