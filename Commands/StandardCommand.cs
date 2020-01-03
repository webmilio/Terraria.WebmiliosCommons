using Terraria;
using Terraria.ModLoader;
using WebmilioCommons.Extensions;

namespace WebmilioCommons.Commands
{
    public abstract class StandardCommand : ModCommand
    {
        protected StandardCommand(string command, CommandType type)
        {
            Command = command;
            Type = type;
        }


        public override bool Autoload(ref string name) => true;


        public override void Action(CommandCaller caller, string input, string[] args)
        {
            Action(caller, caller.Player, input, args);
            caller.Player?.DoIfLocal(player => ActionLocal(caller, player, input, args));
        }

        protected virtual void Action(CommandCaller caller, Player player, string input, string[] args) { }

        protected virtual void ActionLocal(CommandCaller caller, Player player, string input, string[] args) { }


        public override string Command { get; }
        public override CommandType Type { get; }
    }
}
