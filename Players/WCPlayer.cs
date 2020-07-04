﻿using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using WebmilioCommons.Effects.ScreenShaking;
using WebmilioCommons.Extensions;
using WebmilioCommons.Items;
using WebmilioCommons.Items.Standard;
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
        public static WCPlayer Get(ModPlayer modPlayer) => Get(modPlayer.player);


        #region Hooks

        #region Save/Load

        public override TagCompound Save()
        {
            return new TagCompound
            {
                { nameof(UniqueID), UniqueID.ToString() }
            };
        }

        public override void Load(TagCompound tag)
        {
            if (tag.ContainsKey(nameof(UniqueID)))
            {
                string uniqueId = tag.GetString(nameof(UniqueID));

                UniqueID = !string.IsNullOrWhiteSpace(uniqueId) && uniqueId != Guid.Empty.ToString() ? Guid.Parse(uniqueId) : Guid.NewGuid();
            }
            else
                UniqueID = Guid.NewGuid();
        }

        #endregion


        public override bool CanSellItem(NPC vendor, Item[] shopInventory, Item item)
        {
            if (!(item.modItem is ICanBeSold cbs))
                return true;

            return cbs.CanBeSold(this, vendor, shopInventory);
        }


        public override void Initialize()
        {
            InitializeAnimations();
        }


        public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
            IOverridesPlayerDeathMessage opdm = default;


            if (damageSource.SourceNPCIndex > -1)
                opdm = Main.npc[damageSource.SourceNPCIndex]?.modNPC as IOverridesPlayerDeathMessage;
            else if (damageSource.SourceProjectileIndex > -1)
                opdm = Main.projectile[damageSource.SourceProjectileIndex]?.modProjectile as IOverridesPlayerDeathMessage;


            if (opdm != default)
                damageSource.SourceCustomReason = opdm.GetDeathMessage(player, damage, hitDirection, pvp, damageSource);
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


        public override void PostUpdate() => ForAllAnimations(animation => animation.HandlePostUpdate());


        public override void PreUpdate()
        {
            if (!PreUpdateTime())
                return;

            ForAllAnimations(animation => animation.HandlePreUpdate());
        }

        public override void PreUpdateBuffs() => ForAllAnimations(animation => animation.HandlePreUpdateBuffs());
        public override void PreUpdateMovement() => ForAllAnimations(animation => animation.HandlePreUpdateMovements());


        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            /*KeyStates keyState = KeyboardManager.GetKeyState(Keys.U);

            if (keyState != KeyStates.NotPressed)
                Main.NewText($"Key U State: {keyState}");*/
        }


        public override void SetupStartInventory(IList<Item> items, bool mediumcoreDeath)
        {
            foreach (var _mod in ModLoader.Mods.StandardModFilter())
                foreach (var type in _mod.Code.Concrete<IPlayerStartsWith>())
                {
                    var modItem = Activator.CreateInstance(type) as ModItem;

                    if (modItem is IPlayerCanStartWith c && !c.ShouldStartWith(this, player, mediumcoreDeath))
                        continue;


                    modItem.item.SetDefaults(modItem);

                    if (modItem is IPlayerStartsWithStack s)
                        modItem.item.stack = s.StartStack;


                    items.Add(modItem.item);
                }
        }


        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            new WCPlayerOnJoinWorld(this).Send(fromWho, toWho);
        }


        public override void UpdateEquips(ref bool wallSpeedBuff, ref bool tileSpeedBuff, ref bool tileRangeBuff)
        {
            bool
                wallSpeedBuffs2 = wallSpeedBuff,
                tileSpeedBuffs2 = tileSpeedBuff,
                tileRangeBuff2 = tileRangeBuff;

            ForAllAnimations(animation => animation.HandleUpdateEquips(wallSpeedBuffs2, tileSpeedBuffs2, tileRangeBuff2));
        }


        #endregion



        public Guid UniqueID { get; internal set; }
    }
}