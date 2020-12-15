using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using WebmilioCommons.Achievements.Helper;
using WebmilioCommons.Commons;
using WebmilioCommons.Configurations;
using WebmilioCommons.Hooks;
using WebmilioCommons.Hooks.Wiring;
using WebmilioCommons.Identity;
using WebmilioCommons.Inputs;
using WebmilioCommons.ModCompatibilities;
using WebmilioCommons.Networking;
using WebmilioCommons.Networking.Serializing;
using WebmilioCommons.NPCs;
using WebmilioCommons.Players;
using WebmilioCommons.Proxies;
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

            BetterModPlayer.Load();

            ModCompatibilityLoader.Instance.TryLoad();
            NetworkTypeSerializers.Initialize();
        }


        /// <summary></summary>
        public override void Load()
        {
            Proxies.Proxies.Load();

            GlobalNPCSetupShopMethods.Load();
            TimeManagement.Load();

            SpecialNPCNamingBehavior.Load();

            if (Main.netMode != NetmodeID.Server)
            {
                LoadAchievementsMenuHookThingLMAO();
                IdentityManager.Load();
                KeyboardManager.Load();
            }

            #region Hooks

            Main.OnTick += UpdateTick;
            On.Terraria.WorldGen.SaveAndQuit += WorldGen_OnSaveAndQuit;

            Hooking.Load();

            #endregion

            #region Client Configuration

            //ClientConfiguration = ModContent.GetInstance<ClientConfiguration>();

            #endregion

            ModCompatibilityLoader.Instance.OnWCLoadFinished();
        }

        /// <summary></summary>
        public override void PostSetupContent()
        {
            // In theory, you don't need to call the 'TryLoad()' method on any singletons since the instance is loaded the second its created.
            // I only put it there to make it obvious.

            if (Main.netMode != NetmodeID.Server)
            {
                IdentityManager.PostSetupContent();
            }

            Proxies.Proxies.PostSetupContent();
            Hooking.PostSetupContent();

            NetworkPacketLoader.Instance.TryLoad();

            //PlayerSynchronizationPacket.Construct();
        }

        /// <summary></summary>
        public override void Unload()
        {
            BetterModPlayer.Unload();

            // Events

            #region Hooks

            Hooking.Unload();

            Main.OnTick -= UpdateTick;
            On.Terraria.WorldGen.SaveAndQuit -= WorldGen_OnSaveAndQuit;

            #endregion


            GlobalNPCSetupShopMethods.Unload();
            NetworkTypeSerializers.Unload();
            TimeManagement.Unload();

            SpecialNPCNamingBehavior.Unload();

            // Server stuff
            if (Main.netMode != NetmodeID.Server)
            {
                UnloadAchievementsMenuHookThingLMAO();
                IdentityManager.Unload();
                KeyboardManager.Unload();
            }


            // Unload all lazy-loaded singletons.
            List<IUnloadOnModUnload> originalUnloadList = new List<IUnloadOnModUnload>(unloadOnModUnload);
            originalUnloadList.ForEach(toUnload => toUnload.Unload());


            #region Configuration

            //ClientConfiguration = null;

            #endregion


            Proxies.Proxies.Unload();

            Instance = default;
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

        public override void AddRecipes()
        {
            
        }


        private void WorldGen_OnSaveAndQuit(On.Terraria.WorldGen.orig_SaveAndQuit orig, Action callback)
        {
            orig(callback);

            Proxies.Proxies.WorldGen_OnSaveAndQuit();
            TimeManagement.ForceUnalter(false);
        }

        private void UpdateTick() => _ticks++;


        /// <summary>The current loaded instance of <see cref="WebmilioCommonsMod"/>.</summary>
        public static WebmilioCommonsMod Instance { get; private set; }

        //public ClientConfiguration ClientConfiguration { get; private set; }
    }
}