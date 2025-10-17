using System.IO;
using Terraria;
using WebCom.Annotations;

namespace WebCom.Networking;

public abstract class PlayerPacket : Packet
{
    /// <inheritdoc/>
    protected override void ReceivePreMap(BinaryReader reader, int fromWho)
    {
        Player = Main.player[reader.ReadInt32()];
    }

    /// <inheritdoc/>
    protected override void SendPreMap(int toClient = -1, int ignoreClient = -1)
    {
        ModPacket.Write(Player.whoAmI);
    }

    [Skip] public virtual Player Player { get; set; }
}
