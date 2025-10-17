using System.IO;

namespace WebCom.Networking.Serialization;

public interface INetworkSerializable
{
    public void Send(Packet packet);

    public void Receive(Packet packet, BinaryReader reader);
}
