using Terraria.ModLoader;
using WebCom.Net.v2;

namespace WebCom.Tests.Net.v2;

public class MMessage : IMessage { }
public class NMessage : IMessage { }

public class CompoundHandlerMN : IMessageHandler<MMessage>, IMessageHandler<NMessage>
{
  public Mod Mod { get; set; }

  public bool PreReceive(BinaryReader reader, int fromWho, MMessage message)
  {
    return true;
  }

  public bool PreReceive(BinaryReader reader, int fromWho, NMessage message)
  {
    return true;
  }

  public bool PreSend(BinaryWriter writer, MMessage message)
  {
    return true;
  }

  public bool PreSend(BinaryWriter writer, NMessage message)
  {
    return true;
  }

  public void Receive(BinaryReader reader, int fromWho, MMessage message) 
  {
    throw new InvalidOperationException();
  }

  public void Receive(BinaryReader reader, int fromWho, NMessage message) 
  {
    throw new InvalidOperationException();
  }

  public void Send(BinaryWriter writer, MMessage message) 
  { 
  }

  public void Send(BinaryWriter writer, NMessage message) 
  { 
  }
}
