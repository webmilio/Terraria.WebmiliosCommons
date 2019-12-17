using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using WebmilioCommons.Effects.ScreenShaking;
using WebmilioCommons.Time;

namespace WebmilioCommons.Worlds
{
    public sealed class WCWorld : ModWorld
    {
        public override void PostDrawTiles()
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                Update();
        }

        public override void PreUpdate()
        {
            if (Main.netMode == NetmodeID.SinglePlayer || Main.dedServ)
                Update();
        }

        private void Update()
        {
            TimeManagement.Update();
        }
    }
}
