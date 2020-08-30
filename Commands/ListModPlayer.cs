using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using WebmilioCommons.Proxies.Players;

namespace WebmilioCommons.Commands
{
    public sealed class ListModPlayer : DebugCommand
    {
        public ListModPlayer() : base("listmodplayer", CommandType.World)
        {
        }


        protected override void Action(CommandCaller caller, Player player, string input, string[] args)
        {
            foreach (var modPlayer in PlayerHooksProxy.GetModPlayers(player))
            {
                string DisplayModPlayer() => $"{modPlayer.Name}";

                if (Main.netMode == NetmodeID.Server)
                    Console.WriteLine(DisplayModPlayer());
                else
                    Main.NewText(DisplayModPlayer());
            }
        }
    }
}