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
  private static readonly Dictionary<Mod, MessageLoader> loaders = [];
  private static readonly Serializers<BinarySerializer> serializers = new BinarySerializers.DefaultPacketSerializers();

  public static MessageLoader Get(Mod mod)
  {
    return loaders.TryGetValue(mod, out var loader) ?
        loader :
        loaders[mod] = new MessageLoader(mod, serializers);
  }

  private readonly Mod mod;
  private readonly Dictionary<Type, int> msgIds = [];
  private readonly Dictionary<int, IHandlerOrchestrator> orchestrators = [];

  private const int ServerWhoAmI = 255;

  private MessageLoader(Mod mod, Serializers<BinarySerializer> serializers)
  {
    this.mod = mod;

    int nextId = -1;

    var types = mod.Code.GetTypes();
    var msgTypes = types.Concrete<IMessage>();

    foreach (var type in msgTypes)
    {
      msgIds[type] = nextId;
      nextId--;
    }

    var handlerTypes = types.Concrete<IMessageHandler>();
    foreach (var type in handlerTypes)
    {
      var (ids, orchestrator) = GetOrchestrator(type, serializers);
      foreach (var id in ids)
      {
        orchestrators[id] = orchestrator;
      }
    }
  }

  internal (int[] ids, IHandlerOrchestrator orchestrator) GetOrchestrator(Type messageType, Serializers<BinarySerializer> serializers)
  {
    ValidateMessageType(messageType);
    return HandlerOrchestrator.Get(messageType, msgIds, serializers);
  }

  internal static void ValidateMessageType(Type messageType)
  {
    var ctors = messageType.GetConstructors();
    var paramlessCtor = ctors.FirstOrDefault(ctor => ctor.GetParameters().Length == 0) ??
        throw new NotSupportedException($"Could not find parameterless constructor for type {messageType}.");
  }

  internal IHandlerOrchestrator GetOrchestrator(int id)
  {
    return orchestrators[id];
  }

  public void Handle(BinaryReader reader, int whoAmI)
  {
    var type = reader.ReadInt32();
    var handler = GetOrchestrator(type);

    handler.Receive(type, reader, whoAmI);
  }

  // These are not particularly pretty, but they allow me to unit-test the whole thing.
  internal static Func<int> netMode = () => Main.netMode;
  internal static Func<int> myPlayer = () => Main.myPlayer;

  public void Send(IMessage message) => Send(message, -1, -1);

  public void Send(IMessage message, int toClient, int ignoreClient)
  {
    BinaryWriter stream;
    Action finish;

    if (netMode() == NetmodeID.SinglePlayer)
    {
      var memory = new MemoryStream(byte.MaxValue);
      stream = new BinaryWriter(memory);

      // Reset MemoryStream and pretend to be receiving.
      // This ensures behavior is the same on multiplayer and singleplayer.
      finish = delegate
      {
        memory.Seek(0, SeekOrigin.Begin);
        using var reader = new BinaryReader(memory);

        Handle(reader, myPlayer());
      };
    }
    else
    {
      var packet = mod.GetPacket();

      stream = packet;
      finish = () => packet.Send(toClient, ignoreClient);
    }

    var type = msgIds[message.GetType()];
    var orchestrator = GetOrchestrator(type);

    stream.Write(type);

    if (orchestrator.Send(type, stream, message))
    {
      finish();
    }

    stream.Dispose();
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