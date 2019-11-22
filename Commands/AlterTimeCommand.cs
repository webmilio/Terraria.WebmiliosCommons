using Terraria;
using Terraria.ModLoader;
using WebmilioCommons.Time;

namespace WebmilioCommons.Commands
{
    public class AlterTimeCommand : DebugCommand
    {
        public AlterTimeCommand() : base("altertime", CommandType.Chat)
        {
        }

        protected override void Action(CommandCaller caller, Player player, string input, string[] args)
        {
            TimeManagement.TryAlterTime(new TimeAlterationRequest(player, int.Parse(args[0]) * 60, int.Parse(args[1])));
        }
    }
}