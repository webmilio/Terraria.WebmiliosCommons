using Terraria.ModLoader;
using WebCom.Annotations;
using WebCom.Net.v2;

namespace WebCom.Tests.Net.v2;

public class XMessageHandler : IMessageHandler<XMessage>
{
  public Mod Mod { get; set; }

  public bool PreReceive(BinaryReader reader, int fromWho, XMessage message)
  {
    return true;
  }

  public bool PreSend(BinaryWriter writer, XMessage message)
  {
    return true;
  }

  public void Receive(BinaryReader reader, int fromWho, XMessage message)
  {
  }

  public void Send(BinaryWriter writer, XMessage message)
  {
  }
}

public class XMessage : IMessage
{
  [Skip] internal Func<BinaryWriter, bool> OverridePreSend { get; init; } = (writer) => true;
  [Skip] internal Func<BinaryReader, int, bool> OverridePreReceive { get; init; } = (reader, fromWho) => true;
  [Skip] internal Action<BinaryReader, int> OverrideReceive { get; init; } = (reader, fromWho) => { };

  public string AProperty 
  { 
    get; 
    set; 
  } = string.Empty;

  public int NumericalProperty { get; set; }

  public virtual bool PreSend(BinaryWriter writer)
  {
    return OverridePreSend(writer);
  }

  public virtual bool PreReceive(BinaryReader reader, int fromWho)
  {
    return OverridePreReceive(reader, fromWho);
  }

  public virtual void Receive(BinaryReader reader, int fromWho)
  {
    OverrideReceive(reader, fromWho);
  }
}