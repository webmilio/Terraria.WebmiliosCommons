using System.IO;

namespace WebCom.Net.v2;

public interface IHandlerOrchestrator
{
  IMessage Receive(int type, BinaryReader reader, int fromWho);

  bool Send(int type, BinaryWriter writer, IMessage message);
}
