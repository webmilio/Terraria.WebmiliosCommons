using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WebCom.Extensions;
using WebCom.Net.v2.Serialization;
using WebCom.Serializers;

namespace WebCom.Net.v2;

internal class HandlerOrchestrator : IHandlerOrchestrator
{
  internal delegate IMessage CreatorExpression();

  internal delegate bool PreSendExpression(BinaryWriter writer, IMessage message);
  internal delegate void SendExpression(BinaryWriter writer, IMessage message);
  internal delegate bool PreReceiveExpression(BinaryReader reader, int fromWho, IMessage message);
  internal delegate void ReceiveExpression(BinaryReader reader, int fromWho, IMessage message);

  internal delegate void ReaderExpression(BinaryReader reader, object instance);
  internal delegate void WriterExpression(BinaryWriter writer, object instance);

  private readonly CreatorExpression @new;

  private readonly PreSendExpression preSend;
  private readonly SendExpression send;
  private readonly PreReceiveExpression preReceive;
  private readonly ReceiveExpression receive;

  private readonly ReaderExpression read;
  private readonly WriterExpression write;

  private static Type[] GetHandlerInterfaces(Type handlerType)
  {
    var match = typeof(IMessageHandler<>);
    return handlerType.GetInterfaces()
      .Where(iface => iface.IsGenericType && iface.GetGenericTypeDefinition() == match)
      .ToArray();
  }

  public static (int[] ids, IHandlerOrchestrator orchestrator) Get(Type handlerType, IDictionary<Type, int> messageIds, Serializers<BinarySerializer> serializers)
  {
    if (!handlerType.IsAssignableTo(typeof(IMessageHandler)))
    {
      throw new NotSupportedException("The specified type is not a subclass of IMessageHandler.");
    }

    var handler = handlerType.Create<IMessageHandler>();
    var interfaceTypes = GetHandlerInterfaces(handlerType);
    var messageTypes = interfaceTypes.Select(i => i.GetGenericArguments()[0]).ToArray();

    if (interfaceTypes.Length == 0)
    {
      throw new InvalidOperationException($"0 message types registered for type {handlerType.Name}");
    }

    if (interfaceTypes.Length == 1)
    {
      var interfaceType = interfaceTypes[0];
      var messageType = messageTypes[0];

      return (
        [messageIds[messageType]], 
        new HandlerOrchestrator(handler, interfaceType, serializers)
      );
    }

    var orchestrators = new Dictionary<int, IHandlerOrchestrator>();
    for (int i = 0; i < interfaceTypes.Length; i++)
    {
      var interfaceType = interfaceTypes[i];
      var messageType = messageTypes[i];

      var orchestrator = new HandlerOrchestrator(handler, interfaceType, serializers);
      var type = messageIds[messageType];

      orchestrators[type] = orchestrator;
    }

    return (orchestrators.Keys.ToArray(), new CompositeRegistration(orchestrators));
  }

  internal HandlerOrchestrator(IMessageHandler handler, Type interfaceType, Serializers<BinarySerializer> serializers)
  {
    var messageType = interfaceType.GetGenericArguments()[0];

    @new = MessageExpressionHelpers.GetCreator(messageType);
    (read, write) = MessageExpressionHelpers.DeserializerSerializer(messageType, serializers);

    var handlerType = handler.GetType();
    var interfaceMap = handlerType.GetInterfaceMap(interfaceType);
    var mappings = interfaceMap.TargetMethods.ToDictionary(x => x.Name);

    var funcPreReceive = mappings[nameof(IMessageHandler<IMessage>.PreReceive)];
    preReceive = MessageExpressionHelpers.ForDelegate<PreReceiveExpression>(handler, funcPreReceive);

    var funcReceive = mappings[nameof(IMessageHandler<IMessage>.Receive)];
    receive = MessageExpressionHelpers.ForDelegate<ReceiveExpression>(handler, funcReceive);

    var funcPreSend = mappings[nameof(IMessageHandler<IMessage>.PreSend)];
    preSend = MessageExpressionHelpers.ForDelegate<PreSendExpression>(handler, funcPreSend);

    var funcSend = mappings[nameof(IMessageHandler<IMessage>.Send)];
    send = MessageExpressionHelpers.ForDelegate<SendExpression>(handler, funcSend);
  }

  public IMessage Receive(int type, BinaryReader reader, int fromWho)
  {
    var message = @new();

    if (!preReceive(reader, fromWho, message))
    {
      return null;
    }

    read(reader, message);
    receive(reader, fromWho, message);

    return message;
  }

  public bool Send(int type, BinaryWriter writer, IMessage message)
  {
    if (!preSend(writer, message))
    {
      return false;
    }

    write(writer, message);
    send(writer, message);

    return true;
  }

  public class CompositeRegistration(IDictionary<int, IHandlerOrchestrator> orchestrators) : IHandlerOrchestrator
  {
    private readonly IDictionary<int, IHandlerOrchestrator> orchestrators = orchestrators;

    public IMessage Receive(int type, BinaryReader reader, int fromWho)
    {
      return orchestrators[type].Receive(type, reader, fromWho);
    }

    public bool Send(int type, BinaryWriter writer, IMessage message)
    {
      return orchestrators[type].Send(type, writer, message);
    }
  }
}