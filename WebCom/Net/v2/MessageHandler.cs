using System.IO;
using Terraria.ModLoader;

namespace WebCom.Net.v2;

public class MessageHandler<T> : IMessageHandler<T> where T : IMessage
{
  public Mod Mod { get; set; }

  public virtual bool PreReceive(BinaryReader reader, int fromWho, T message)
  {
    return true;
  }

  public virtual bool PreSend(BinaryWriter writer, T message)
  {
    return true;
  }

  public virtual void Receive(BinaryReader reader, int fromWho, T message)
  {
  }

  public virtual void Send(BinaryWriter writer, T message)
  {
  }
}
