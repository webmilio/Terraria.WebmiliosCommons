using Terraria.ModLoader;
using WebCom.Net.v2;

namespace WebCom.Tests.Net.v2;

public class EmptyMessage : IMessage
{
}

public class EmptyMessageHandler : IMessageHandler<EmptyMessage>
{
  public Mod Mod { get; set; }

  public bool PreReceive(BinaryReader reader, int fromWho, EmptyMessage message)
  {
    return true;
  }

  public bool PreSend(BinaryWriter writer, EmptyMessage message)
  {
    return true;
  }

  public void Receive(BinaryReader reader, int fromWho, EmptyMessage message)
  {
    throw new Exception("Success");
  }

  public void Send(BinaryWriter writer, EmptyMessage message)
  {
    throw new Exception("Success");
  }
}