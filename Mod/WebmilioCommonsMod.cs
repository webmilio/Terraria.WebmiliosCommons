using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using WebmilioCommons.Commons;
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
            LoadHooks();

            NetworkTypeSerializers.Initialize();
            
            GlobalNPCSetupShopMethods.Load();
            SpecialNPCNamingBehavior.Load();
        }

        public override void PostSetupContent()
        {
            Logger.InfoFormat("Initialized {0} with {1} network packets.", nameof(NetworkPacketLoader), NetworkPacketLoader.Instance.Count);
        }

        /// <summary></summary>
        public override void Unload()
        {
            BetterModNPC.Unload();

            GlobalNPCSetupShopMethods.Unload();
            NetworkTypeSerializers.Unload();

            SpecialNPCNamingBehavior.Unload();

            // Unload all lazy-loaded singletons.
            List<IUnloadOnModUnload> originalUnloadList = new List<IUnloadOnModUnload>(unloadOnModUnload);
            originalUnloadList.ForEach(toUnload => toUnload.Unload());

            #region Configuration

            //ClientConfiguration = null;

            #endregion

            UnloadHooks();

            Instance = default;
        }


        /// <summary></summary>
        /// <param name="reader"></param>
        /// <param name="whoAmI"></param>
        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            NetworkPacketLoader.Instance.HandlePacket(reader, whoAmI);
        }

        /// <summary>The current loaded instance of <see cref="WebmilioCommonsMod"/>.</summary>
        public static WebmilioCommonsMod Instance { get; private set; }
    }
}