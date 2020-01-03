using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using WebmilioCommons.Achievements;
using WebmilioCommons.Achievements.Helper;
using WebmilioCommons.Commons;
using WebmilioCommons.Configurations;
using WebmilioCommons.Identity;
using WebmilioCommons.Inputs;
using WebmilioCommons.Loaders;
using WebmilioCommons.Networking;
using WebmilioCommons.Rarities;
using WebmilioCommons.Time;

namespace WebmilioCommons
{
    public sealed partial class WebmilioCommonsMod : Mod
    {
        internal List<IUnloadOnModUnload> unloadOnModUnload = new List<IUnloadOnModUnload>();


        public WebmilioCommonsMod()
        {
            Instance = this;
        }


        public override void Load()
        {
            KeyboardManager.Load();
            TimeManagement.Load();
            //ModRarityLoader.Instance.TryLoad();


            if (!Main.dedServ)
            {
                IdentityManager.Load();
            }


            On.Terraria.WorldGen.SaveAndQuit += WorldGenOnSaveAndQuit;

            #region Client Configuration

            ClientConfiguration = ModContent.GetInstance<ClientConfiguration>();

            #endregion
        }

        public override void PostSetupContent()
        {
            // In theory, you don't need to call the 'TryLoad()' method on any singletons since the instance is loaded the second its created.
            // I only put it there to make it obvious.

            if (Main.netMode != NetmodeID.Server)
            {
                IdentityManager.PostSetupContent();

                ModAchievementHelper.PostSetupContent();
            }

            NetworkPacketLoader.Instance.TryLoad();

            //PlayerSynchronizationPacket.Construct();
        }

        public override void Unload()
        {
            On.Terraria.WorldGen.SaveAndQuit -= WorldGenOnSaveAndQuit;

            TimeManagement.Unload();


            if (Main.netMode != NetmodeID.Server)
            {
                IdentityManager.Unload();

                ModAchievementHelper.Unload();
            }


            List<IUnloadOnModUnload> originalUnloadList = new List<IUnloadOnModUnload>(unloadOnModUnload);
            originalUnloadList.ForEach(toUnload => toUnload.Unload());


            #region Configuration

            ClientConfiguration = null;

            #endregion

            Instance = null;
        }

        private void WorldGenOnSaveAndQuit(On.Terraria.WorldGen.orig_SaveAndQuit orig, Action callback)
        {
            orig(callback);

            TimeManagement.ForceUnalter(false);
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

        public ClientConfiguration ClientConfiguration { get; private set; }
    }
}