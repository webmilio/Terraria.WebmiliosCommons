using System.IO;

namespace WebCom.Net.v2;

public interface IMessage
{
    bool PreSend();

    bool PreReceive(BinaryReader reader, int fromWho);

    void Receive(BinaryReader reader, int fromWho);
}
