using Terraria.ModLoader;

namespace WebmilioCommons.Commands
{
    public abstract class DebugCommand : StandardCommand
    {
        protected DebugCommand(string command, CommandType type) : base(command, type)
        {
        }

        public override bool IsLoadingEnabled(Mod mod)
        {
            return false;
        }
    }
}