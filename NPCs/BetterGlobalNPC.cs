using Terraria;
using Terraria.ModLoader;

namespace WebmilioCommons.NPCs
{
    public abstract class BetterGlobalNPC : GlobalNPC
    {
        /// <summary>Overwritten by <see cref="BetterGlobalNPC"/>. Override <see cref="ModSetupShop"/> to define custom behavior.</summary>
        /// <param name="type">The NPC.</param>
        /// <param name="shop"></param>
        /// <param name="nextSlot"></param>
        public sealed override void SetupShop(int type, Chest shop, ref int nextSlot)
        {
            if (GlobalNPCSetupShopMethods.shopMethods.ContainsKey(type))
                GlobalNPCSetupShopMethods.shopMethods[type](this)(shop, ref nextSlot);

            ModSetupShop(type, shop, ref nextSlot);
        }

        /// <inheritdoc cref="GlobalNPC.SetupShop(int,Terraria.Chest,ref int)"/>
        public virtual void ModSetupShop(int type, Chest shop, ref int nextSlot) { }


#pragma warning disable 1591
        public virtual void SetupArmsDealerShop(Chest shop, ref int nextSlot) { }
        public virtual void SetupClothierShop(Chest shop, ref int nextSlot) { }
        public virtual void SetupCyborgShop(Chest shop, ref int nextSlot) { }
        public virtual void SetupDemolitionistShop(Chest shop, ref int nextSlot) { }
        public virtual void SetupDryadShop(Chest shop, ref int nextSlot) { }
        public virtual void SetupDyeTraderShop(Chest shop, ref int nextSlot) { }
        public virtual void SetupGoblinTinkererShop(Chest shop, ref int nextSlot) { }
        public virtual void SetupMechanicShop(Chest shop, ref int nextSlot) { }
        public virtual void SetupMerchantShop(Chest shop, ref int nextSlot) { }
        public virtual void SetupPainterShop(Chest shop, ref int nextSlot) { }
        public virtual void SetupPartyGirlShop(Chest shop, ref int nextSlot) { }
        public virtual void SetupPirateShop(Chest shop, ref int nextSlot) { }
        public virtual void SetupSantaClausShop(Chest shop, ref int nextSlot) { }
        public virtual void SetupSteampunkerShop(Chest shop, ref int nextSlot) { }
        public virtual void SetupStylistShop(Chest shop, ref int nextSlot) { }
        public virtual void SetupTavernkeepShop(Chest shop, ref int nextSlot) { }
        public virtual void SetupTruffleShop(Chest shop, ref int nextSlot) { }
        public virtual void SetupWitchDoctorShop(Chest shop, ref int nextSlot) { }
        public virtual void SetupWizardShop(Chest shop, ref int nextSlot) { }
#pragma warning restore 1591
    }
}
