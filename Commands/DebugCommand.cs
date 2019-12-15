using Terraria;
using Terraria.ModLoader;

namespace WebmilioCommons.Commands
{
    public abstract class DebugCommand : ModCommand
    {
        protected DebugCommand(string command, CommandType type)
        {
            Command = command;
            Type = type;
        }


        public override bool Autoload(ref string name) => true;


        public override void Action(CommandCaller caller, string input, string[] args)
        {
            Action(caller, caller.Player, input, args);
        }

        protected virtual void Action(CommandCaller caller, Player player, string input, string[] args) { }


        public override string Command { get; }
        public override CommandType Type { get; }
    }
}