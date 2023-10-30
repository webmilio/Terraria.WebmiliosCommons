using Terraria;
using Terraria.ModLoader;
using WebCom.Networking;

namespace WebCom.Content;

internal class WebComPlayer : ModPlayer
{
    public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
    {
        if (Main.dedServ)
        {
            Mod.GetPacket<WebComWorld.WorldSynchronizePacket>().Send(fromWho);
        }
    }
}