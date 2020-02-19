using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using WebmilioCommons.Achievements.Helper;
using WebmilioCommons.Commons;
using WebmilioCommons.Configurations;
using WebmilioCommons.Identity;
using WebmilioCommons.Inputs;
using WebmilioCommons.Networking;
using WebmilioCommons.Networking.Serializing;
using WebmilioCommons.Rarities;
using WebmilioCommons.Time;

namespace WebmilioCommons
{
    public sealed partial class WebmilioCommonsMod : Mod
    {
        private ulong _ticks;
        internal List<IUnloadOnModUnload> unloadOnModUnload = new List<IUnloadOnModUnload>();


        public WebmilioCommonsMod()
        {
            Instance = this;

            NetworkTypeSerializers.Initialize();
        }


        /// <summary></summary>
        public override void Load()
        {
            KeyboardManager.Load();
            TimeManagement.Load();
            ModRarityLoader.Instance.TryLoad();

            if (!Main.dedServ)
            {
                IdentityManager.Load();
            }


            Main.OnTick += UpdateTick;
            On.Terraria.WorldGen.SaveAndQuit += WorldGenOnSaveAndQuit;


            #region Client Configuration

            //ClientConfiguration = ModContent.GetInstance<ClientConfiguration>();

            #endregion
        }

        /// <summary></summary>
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

        /// <summary></summary>
        public override void Unload()
        {
            // Events
            Main.OnTick -= UpdateTick;
            On.Terraria.WorldGen.SaveAndQuit -= WorldGenOnSaveAndQuit;

            NetworkTypeSerializers.Unload();
            TimeManagement.Unload();


            // Server stuff
            if (Main.netMode != NetmodeID.Server)
            {
                IdentityManager.Unload();

                ModAchievementHelper.Unload();
            }


            // Unload all lazy-loaded singletons.
            List<IUnloadOnModUnload> originalUnloadList = new List<IUnloadOnModUnload>(unloadOnModUnload);
            originalUnloadList.ForEach(toUnload => toUnload.Unload());


            #region Configuration

            //ClientConfiguration = null;

            #endregion

            Instance = null;
        }


        /// <summary></summary>
        /// <param name="reader"></param>
        /// <param name="whoAmI"></param>
        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            NetworkPacketLoader.Instance.HandlePacket(reader, whoAmI);
        }

        /// <summary></summary>
        public override void PostUpdateInput()
        {
            KeyboardManager.Update();
        }


        private void WorldGenOnSaveAndQuit(On.Terraria.WorldGen.orig_SaveAndQuit orig, Action callback)
        {
            orig(callback);

            TimeManagement.ForceUnalter(false);
        }

        private void UpdateTick() => _ticks++;


        /// <summary>The current loaded instance of <see cref="WebmilioCommonsMod"/>.</summary>
        public static WebmilioCommonsMod Instance { get; private set; }

        //public ClientConfiguration ClientConfiguration { get; private set; }
    }
}