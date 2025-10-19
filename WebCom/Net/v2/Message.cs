using System.IO;

namespace WebCom.Net.v2;

public class Message : IMessage
{
    public virtual bool PreReceive(BinaryReader reader, int fromWho)
    {
        return true;
    }

    public virtual bool PreSend()
    {
        return true;
    }

    public virtual void Receive(BinaryReader reader, int fromWho)
    {
    }
}
