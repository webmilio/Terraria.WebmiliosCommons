using System.IO;
using Terraria;
using Terraria.ModLoader;
using WebmilioCommons.Identity;
using WebmilioCommons.Inputs;
using WebmilioCommons.Networking;
using WebmilioCommons.Networking.Packets;
using WebmilioCommons.Time;

namespace WebmilioCommons
{
    public sealed partial class WebmilioCommonsMod : Mod
	{
		public WebmilioCommonsMod()
        {
            Instance = this;
        }


        public override void Load()
        {
            KeyboardManager.Load();

            if (!Main.dedServ)
                IdentityManager.Load();

            NetworkPacketLoader.Instance.Load();

            TimeManagement.Load();
        }

        public override void PostSetupContent()
        {
            if (!Main.dedServ)
                IdentityManager.PostSetupContent();

            //PlayerSynchronizationPacket.Construct();
        }

        public override void Unload()
        {
            TimeManagement.Unload();

            NetworkPacketLoader.Instance.Unload();
            IdentityManager.Unload();

            Instance = null;
        }


        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            NetworkPacketLoader.Instance.HandlePacket(reader, whoAmI);
        }

        public override void PostUpdateInput()
        {
            KeyboardManager.Update();
        }


        public static WebmilioCommonsMod Instance { get; private set; }
	}
}