﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using WebmilioCommons.Extensions;
using WebmilioCommons.States;

namespace WebmilioCommons.Time
{
    /// <summary>Work-in-progress, feedback is highly appreciated on any crashes found.</summary>
    public static class TimeManagement
    {
        internal static List<string> bannedMods = new List<string>();

        private static List<int> _npcs, _projectiles, _items;

        internal static Dictionary<NPC, NPCInstantState> npcStates;
        internal static Dictionary<Projectile, ProjectileInstantState> projectileStates;
        internal static Dictionary<Player, PlayerInstantState> playerStates;
        internal static Dictionary<Item, ItemInstantState> itemStates;

        #region Immunity

        #region NPC Immunity

        public static void AddNPCImmunity<T>() where T : ModNPC => AddNPCImmunity(ModContent.NPCType<T>());

        public static void AddNPCImmunity(int type)
        {
            ModNPC modNPC = ModContent.GetModNPC(type);

            if (modNPC != null && IsModBanned(modNPC.GetType()))
                return;

            _npcs.Add(type);
        }


        // TODO Not really efficient, find a way to make this faster.
        public static bool IsNPCImmune<T>() where T : ModNPC => IsNPCImmune(ModContent.NPCType<T>());
        public static bool IsNPCImmune(NPC npc) => IsNPCImmune(npc.type);

        public static bool IsNPCImmune(int type)
        {
            ModNPC modNPC = ModContent.GetModNPC(type);

            if (modNPC != null && IsModBanned(modNPC.GetType()))
                return false;

            return _npcs.Contains(type);
        }

        #endregion


        #region Projectile Immunity

        public static void AddProjectileImmunity<T>() where T : ModProjectile => AddProjectileImmunity(ModContent.ProjectileType<T>());

        public static void AddProjectileImmunity(int type)
        {
            ModProjectile modProjectile = ModContent.GetModProjectile(type);

            if (modProjectile != null && IsModBanned(modProjectile.GetType()))
                return;

            _projectiles.Add(type);
        }


        public static bool IsProjectileImmune<T>() where T : ModProjectile => IsProjectileImmune(ModContent.ProjectileType<T>());
        public static bool IsProjectileImmune(ModProjectile modProjectile) => IsProjectileImmune(modProjectile.projectile);
        public static bool IsProjectileImmune(Projectile projectile) => IsProjectileImmune(projectile.type);

        public static bool IsProjectileImmune(int type)
        {
            ModProjectile modProjectile = ModContent.GetModProjectile(type);

            if (modProjectile != null && IsModBanned(modProjectile.GetType()))
                return false;

            return _projectiles.Contains(type);
        }

        #endregion


        #region Item Immunity

        public static void AddItemImmunity<T>() where T : ModItem => AddItemImmunity(ModContent.ItemType<T>());

        public static void AddItemImmunity(int type)
        {
            ModItem modItem = ModContent.GetModItem(type);

            if (modItem != null && IsModBanned(modItem.GetType()))
                return;

            _items.Add(type);
        }


        public static bool IsItemImmune<T>() where T : ModItem => IsItemImmune(ModContent.ItemType<T>());
        public static bool IsItemImmune(ModItem modItem) => IsItemImmune(modItem.item);
        public static bool IsItemImmune(Item item) => IsItemImmune(item.type);

        public static bool IsItemImmune(int type)
        {
            ModItem modItem = ModContent.GetModItem(type);

            if (modItem != null && IsModBanned(modItem.GetType()))
                return false;

            return _items.Contains(type);
        }

        #endregion


        #region Immunity Banning
        [Obsolete("Work-in-progress", false)]
        public static void RequestImmunityModBan(Mod mod) => RequestImmunityModBan(mod.Name);

        [Obsolete("Work-in-progress", false)]
        public static void RequestImmunityModBan(string modName)
        {
            if (!bannedMods.Contains(modName))
                bannedMods.Add(modName);

            WebmilioCommonsMod.Instance.Logger.InfoFormat("Mod `{0}` requested mod {1} to be banned from requesting NPC immunity.",
                new StackTrace().GetFirstDifferentAssembly().GetModFromAssembly().Name, modName);
        }

        private static bool IsModBanned<T>() => IsModBanned(typeof(T));
        private static bool IsModBanned(Type type) => bannedMods.Contains(type.GetModFromType().Name);
        #endregion

        #endregion


        public static bool TryAlterTime(TimeAlterationRequest request, bool local = true)
        {
            if (!VerifyModificationLock(request))
                return false;

            AlterTime(request, local);
            return true;
        }


        private static bool AlterTime(TimeAlterationRequest request, bool local = true)
        {
            CurrentRequest = CurrentRequest.Duration == 0 ? null : request;


            for (int i = 0; i < Main.npc.Length; i++)
            {
                NPC npc = Main.npc[i];

                if (!npc.active || IsNPCImmune(npc) || npc.modNPC is IIsNPCImmune immuneNPC && immuneNPC.IsImmune(npc, request))
                    continue;

                RegisterStoppedNPC(npc);
            }


            if (local)
                NotifyTimeAlter(request);

            return true;
        }


        #region Unalter

        public static bool TryUnalterTime(Player player) => TryAlterTime(GenerateUnalterRequest(player));
        public static bool TryUnalterTime(NPC npc) => TryAlterTime(GenerateUnalterRequest(npc));
        public static bool TryUnalterTime(Projectile projectile) => TryAlterTime(GenerateUnalterRequest(projectile));
        public static bool TryUnalterTime(Item item) => TryAlterTime(GenerateUnalterRequest(item));

        public static bool TryUnalterTime(TimeAlterationRequest.Sources source)
        {
            if (source.HasFlag(TimeAlterationRequest.Sources.Entity))
                throw new ArgumentException("Use a function that takes an Entity when creating a request from an entity.");

            return TryAlterTime(GenerateUnalterRequest(source));
        }


        public static TimeAlterationRequest GenerateUnalterRequest(Player player) => new TimeAlterationRequest(player, 0, 1);
        public static TimeAlterationRequest GenerateUnalterRequest(NPC npc) => new TimeAlterationRequest(npc, 0, 1);
        public static TimeAlterationRequest GenerateUnalterRequest(Projectile projectile) => new TimeAlterationRequest(projectile, 0, 1);
        public static TimeAlterationRequest GenerateUnalterRequest(Item item) => new TimeAlterationRequest(item, 0, 1);

        public static TimeAlterationRequest GenerateUnalterRequest(TimeAlterationRequest.Sources source) => new TimeAlterationRequest(source, 0, 1);

        #endregion


        public static bool VerifyModificationLock(TimeAlterationRequest request) => 
            CurrentRequest == null || !CurrentRequest.LockedToSource || 
            CurrentRequest.SourceType == request.SourceType && CurrentRequest.StoppedByEntity == request.StoppedByEntity;

        private static void NotifyTimeAlter(TimeAlterationRequest request)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
                return;

            TimeAlteredPacket packet = new TimeAlteredPacket()
            {
                Source = request.SourceType.ToString(),
                SourceEntity = request.SourceEntity.whoAmI,

                TickRate = request.TickRate,
                Duration = request.Duration,
            };

            packet.Send();
        }


        public static void RegisterStoppedNPC(NPC npc) => npcStates.Add(npc, new NPCInstantState(npc));
        public static void RegisterStoppedProjectile(Projectile projectile) => projectileStates.Add(projectile, new ProjectileInstantState(projectile));
        public static void RegisterStoppedPlayer(Player player) => playerStates.Add(player, new PlayerInstantState(player));
        public static void RegisterStoppedItem(Item item) => itemStates.Add(item, new ItemInstantState(item));


        internal static void Update()
        {
            int previousTimer = TimeAlteredFor;
        }


        internal static void Load()
        {
            _npcs = new List<int>();
            _projectiles = new List<int>();
            _items = new List<int>();
        }

        internal static void Unload()
        {
            _npcs = null;
            _projectiles = null;
            _items = null;
        }


        public static TimeAlterationRequest CurrentRequest { get; private set; }

        public static int TimeAlteredFor { get; private set; }
        public static bool TimeAltered => TimeAlteredFor > 0;

        public static double MainTime { get; internal set; }
        public static double MainRainTime { get; internal set; }
    }
}