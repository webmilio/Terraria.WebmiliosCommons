using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using WebmilioCommons.Effects.ScreenShaking;
using WebmilioCommons.Items;
using WebmilioCommons.Items.Starting;
using WebmilioCommons.Networking.Attributes;
using WebmilioCommons.NPCs;

#pragma warning disable 1591

namespace WebmilioCommons.Players
{
    [AutoNetworkMapping]
    public sealed partial class WCPlayer : ModPlayer
    {
        public static WCPlayer Get() => Get(Main.LocalPlayer);
        public static WCPlayer Get(Player player) => player.GetModPlayer<WCPlayer>();
        public static WCPlayer Get(ModPlayer modPlayer) => Get(modPlayer.Player);


        #region Hooks

        #region Save/Load

        public override void SaveData(TagCompound tag)
        {
            tag.Add(nameof(UniqueId), UniqueId.ToString());
        }

        public override void LoadData(TagCompound tag)
        {
            if (tag.ContainsKey(nameof(UniqueId)))
            {
                string uniqueId = tag.GetString(nameof(UniqueId));

                UniqueId = !string.IsNullOrWhiteSpace(uniqueId) && uniqueId != Guid.Empty.ToString() ? Guid.Parse(uniqueId) : Guid.NewGuid();
            }
            else
                UniqueId = Guid.NewGuid();
        }

        #endregion


        public override bool CanSellItem(NPC vendor, Item[] shopInventory, Item item)
        {
            if (!(item.ModItem is ICanBeSold cbs))
                return true;

            return cbs.CanBeSold(this, vendor, shopInventory);
        }

        public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
            IOverridesPlayerDeathMessage opdm = default;


            if (damageSource.SourceNPCIndex > -1)
                opdm = Main.npc[damageSource.SourceNPCIndex]?.ModNPC as IOverridesPlayerDeathMessage;
            else if (damageSource.SourceProjectileIndex > -1)
                opdm = Main.projectile[damageSource.SourceProjectileIndex]?.ModProjectile as IOverridesPlayerDeathMessage;


            if (opdm != default)
                damageSource.SourceCustomReason = opdm.GetDeathMessage(Player, damage, hitDirection, pvp, damageSource);
        }


        public override void ModifyScreenPosition()
        {
            foreach (ScreenShake screenShake in ScreenShake.Current)
            {
                Main.screenPosition.X += Main.rand.Next(-screenShake.Intensity, screenShake.Intensity);
                Main.screenPosition.Y += Main.rand.Next(-screenShake.Intensity, screenShake.Intensity);
            }

            ScreenShake.TickScreenShakes();
        }

        public override IEnumerable<Item> AddStartingItems(bool mediumCoreDeath)
        {
            List<Item> items = new();

            foreach (var mod in ModStore.Mods)
            foreach (var modItem in mod.GetContent<ModItem>())
            {
                if (modItem is not IPlayerStartsWith)
                    continue;

                if (modItem is IPlayerCanStartWith c && !c.ShouldStartWith(this, Player, mediumCoreDeath))
                    continue;

                Item item = new(modItem.Type);

                if (modItem is IPlayerStartsWithStack s)
                    item.stack = s.StartStack;
                else
                    item.stack = 1;
                
                items.Add(item);
            }

            return items;
        }

        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            new WCPlayerOnJoinWorld(this).Send(fromWho, toWho);
        }

        #endregion


        [Obsolete("Moved to UniqueId", true)]
        public Guid UniqueID => UniqueId;

        public Guid UniqueId { get; internal set; }
    }
}