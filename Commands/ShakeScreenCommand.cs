using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using WebmilioCommons.Effects.ScreenShaking;

namespace WebmilioCommons.Commands
{
    public sealed class ShakeScreenCommand : DebugCommand
    {
        public ShakeScreenCommand() : base("shakescreen", CommandType.Chat)
        {
        }


        protected override void Action(CommandCaller caller, Player player, string input, string[] args)
        {
            int intensity = int.Parse(args[0]);
            int duration = int.Parse(args[1]);
            bool slowsDown = args.Length < 3 || bool.Parse(args[2]);

            Main.NewText($"Shaking screen with intensity:{intensity} duration:{duration}.", Color.Green);
            ScreenShake.ShakeScreen(intensity, duration, slowsDown);
        }
    }
}