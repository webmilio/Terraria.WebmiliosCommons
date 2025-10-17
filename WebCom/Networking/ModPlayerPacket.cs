using Terraria;
using Terraria.ModLoader;
using WebCom.Annotations;

namespace WebCom.Networking;

public abstract class ModPlayerPacket<T> : PlayerPacket where T : ModPlayer
{
    protected virtual T GetModPlayer(Player player)
    {
        return player.GetModPlayer<T>();
    }

    [Skip] public override Player Player
    {
        get => ModPlayer.Player;
        set => ModPlayer = GetModPlayer(value);
    }

    [Skip] public T ModPlayer { get; set; }
}