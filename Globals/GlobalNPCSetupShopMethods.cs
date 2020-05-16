using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace WebmilioCommons.Globals
{
    public static class GlobalNPCSetupShopMethods
    {
        public delegate void NPCSetupShopMethod(Chest shop, ref int nextSlot);

        internal static Dictionary<int, Func<BetterGlobalNPC, NPCSetupShopMethod>> shopMethods;


        internal static void Load()
        {
            shopMethods = new Dictionary<int, Func<BetterGlobalNPC, NPCSetupShopMethod>>();

            AddSetupShopMethod(NPCID.ArmsDealer, npc => npc.SetupArmsDealerShop);
            AddSetupShopMethod(NPCID.Clothier, npc => npc.SetupClothierShop);
            AddSetupShopMethod(NPCID.Cyborg, npc => npc.SetupCyborgShop);
            AddSetupShopMethod(NPCID.DD2Bartender, npc => npc.SetupTavernkeepShop);
            AddSetupShopMethod(NPCID.Demolitionist, npc => npc.SetupDemolitionistShop);
            AddSetupShopMethod(NPCID.Dryad, npc => npc.SetupDryadShop);
            AddSetupShopMethod(NPCID.DyeTrader, npc => npc.SetupDyeTraderShop);
            AddSetupShopMethod(NPCID.GoblinTinkerer, npc => npc.SetupGoblinTinkererShop);
            AddSetupShopMethod(NPCID.Mechanic, npc => npc.SetupMechanicShop);
            AddSetupShopMethod(NPCID.Merchant, npc => npc.SetupMerchantShop);
            AddSetupShopMethod(NPCID.Painter, npc => npc.SetupPainterShop);
            AddSetupShopMethod(NPCID.PartyGirl, npc => npc.SetupPartyGirlShop);
            AddSetupShopMethod(NPCID.Pirate, npc => npc.SetupPirateShop);
            AddSetupShopMethod(NPCID.SantaClaus, npc => npc.SetupSantaClausShop);
            AddSetupShopMethod(NPCID.Steampunker, npc => npc.SetupSteampunkerShop);
            AddSetupShopMethod(NPCID.Stylist, npc => npc.SetupStylistShop);
            AddSetupShopMethod(NPCID.Truffle, npc => npc.SetupTruffleShop);
            AddSetupShopMethod(NPCID.WitchDoctor, npc => npc.SetupWitchDoctorShop);
            AddSetupShopMethod(NPCID.Wizard, npc => npc.SetupWizardShop);
        }

        internal static void Unload()
        {
            shopMethods?.Clear();
            shopMethods = null;
        }

        /// <summary>Adds a method to call for a specific NPC during <see cref="BetterGlobalNPC.SetupShop"/>. Call this during your mod's <see cref="Mod.PostSetupContent"/></summary>
        /// <param name="type">The NPC Type.</param>
        /// <param name="setupShopMethod">The shop method.</param>
        /// <seealso cref="BetterGlobalNPC.SetupShop"/>
        public static void AddSetupShopMethod(int type, Func<BetterGlobalNPC, NPCSetupShopMethod> setupShopMethod) => shopMethods.Add(type, setupShopMethod);
    }
}