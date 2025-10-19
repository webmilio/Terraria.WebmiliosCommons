using System.IO;
using Terraria.ModLoader;

namespace WebCom.Net.v2.Serialization;

public interface INetworkSerializable
{
    public void Send(BinaryWriter writer);

    public void Receive(BinaryReader reader);
}
