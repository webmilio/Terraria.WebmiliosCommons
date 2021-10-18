using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using WebmilioCommons.Commons;
using WebmilioCommons.Identity;
using WebmilioCommons.ModCompatibilities;
using WebmilioCommons.Networking;
using WebmilioCommons.Networking.Serializing;
using WebmilioCommons.NPCs;

namespace WebmilioCommons
{
    public sealed partial class WebmilioCommonsMod : Mod
    {
        public static GameCulture EnglishCulture { get; } = GameCulture.FromName(GameCulture.CultureName.English.ToString());

        internal List<IUnloadOnModUnload> unloadOnModUnload = new();

        public WebmilioCommonsMod()
        {
            Instance = this;
            
            BetterModNPC.Load();
            //BetterModWorld.Unload();
        }


        /// <summary></summary>
        public override void Load()
        {
            ModCompatibilityLoader.Instance.TryLoad();
            NetworkTypeSerializers.Initialize();
            
            GlobalNPCSetupShopMethods.Load();
            TimeManagement.Load();

            SpecialNPCNamingBehavior.Load();

            if (Main.netMode != NetmodeID.Server)
            {
                IdentityManager.Load();
            }

            #region Hooks

            On.Terraria.WorldGen.SaveAndQuit += WorldGen_OnSaveAndQuit;
            
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
            
            NetworkPacketLoader.Instance.TryLoad();

            //PlayerSynchronizationPacket.Construct();
        }

        /// <summary></summary>
        public override void Unload()
        {
            BetterModNPC.Unload();

            // Events
            #region Hooks
            
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
                //KeyboardManager.Unload();
            }


            // Unload all lazy-loaded singletons.
            List<IUnloadOnModUnload> originalUnloadList = new List<IUnloadOnModUnload>(unloadOnModUnload);
            originalUnloadList.ForEach(toUnload => toUnload.Unload());

            #region Configuration

            //ClientConfiguration = null;

            #endregion

            Instance = default;
        }


        /// <summary></summary>
        /// <param name="reader"></param>
        /// <param name="whoAmI"></param>
        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            NetworkPacketLoader.Instance.HandlePacket(reader, whoAmI);
        }

        private void WorldGen_OnSaveAndQuit(On.Terraria.WorldGen.orig_SaveAndQuit orig, Action callback)
        {
            orig(callback);

            TimeManagement.ForceUnalter(false);
        }

        /// <summary>The current loaded instance of <see cref="WebmilioCommonsMod"/>.</summary>
        public static WebmilioCommonsMod Instance { get; private set; }
    }
}