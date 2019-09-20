using System.IO;
using Terraria.ModLoader;

namespace WebmilioCommons.Networking
{
    public interface INetworkSerializable
    {
        void Send(ModPacket modPacket);

        void Receive(BinaryReader reader);
    }
}