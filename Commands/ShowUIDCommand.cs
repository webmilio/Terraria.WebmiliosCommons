using Terraria;
using Terraria.ModLoader;
using WebmilioCommons.Players;
using WebmilioCommons.Tinq;

namespace WebmilioCommons.Commands
{
    public class ShowUIDCommand : DebugCommand
    {
        public ShowUIDCommand() : base("showUID", CommandType.Chat)
        {
        }


        protected override void ActionLocal(CommandCaller caller, Player player, string input, string[] args)
        {
            Main.player.DoActive(plr =>
            {
                Main.NewText($"{plr.name}: {WCPlayer.Get(plr).UniqueId}");
            });
        }
    }
}