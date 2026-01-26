using System;
using System.IO;
using Terraria.ModLoader;

namespace WebCom.Net.v2;

public interface IMessageHandler
{
  Mod Mod { get; set; }
}

public interface IMessageHandler<T> : IMessageHandler where T : IMessage
{
  bool PreSend(BinaryWriter writer, T message);

  void Send(BinaryWriter writer, T message);

  bool PreReceive(BinaryReader reader, int fromWho, T message);

  void Receive(BinaryReader reader, int fromWho, T message);
}
