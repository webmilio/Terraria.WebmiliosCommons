using System;
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

        /// <summary></summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public override bool IsLoadingEnabled(Mod mod)
        {
            string x = null;
            return Autoload(ref x);
        }

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            Action(caller, caller.Player, input, args);
            caller.Player?.DoIfLocal(player => ActionLocal(caller, player, input, args));
        }

        protected virtual void Action(CommandCaller caller, Player player, string input, string[] args) { }

        protected virtual void ActionLocal(CommandCaller caller, Player player, string input, string[] args) { }


        public override string Command { get; }
        public override CommandType Type { get; }

        [Obsolete("Override IsLoadingEnabled.")]
        public virtual bool Autoload(ref string name) => true;
    }
}
