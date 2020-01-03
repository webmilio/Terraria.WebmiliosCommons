using System;
using System.Collections.Generic;
using System.Diagnostics;
using Terraria;
using Terraria.Achievements;
using Terraria.GameContent.Achievements;
using Terraria.ID;
using Terraria.ModLoader;
using WebmilioCommons.Achievements.Helper;
using WebmilioCommons.Extensions;

namespace WebmilioCommons.Achievements
{
    public class ModAchievement
    {
        public const string DEFAULT_COMPLETION_FLAG = "Completed";
        internal readonly List<AchievementCondition> conditions = new List<AchievementCondition>();


        public ModAchievement(string name, string description, AchievementCategory category)
        {
            Name = name;
            Description = description;

            Category = category;

            Type type = GetType();
            TexturePath = $"{type.GetModFromType().Name}/{type.GetPath()}";
        }


        public virtual void SetDefaults()
        {

        }


        public void AddCondition(AchievementCondition condition)
        {
            conditions.Add(condition);
        }

        public void AddConditions(params AchievementCondition[] conditions)
        {
            for (int i = 0; i < conditions.Length; i++)
                AddCondition(conditions[i]);
        }


        /// <summary>Adds a default completion flag to be manually trigger with <see cref="CompleteFlag()"/>.</summary>
        public void AddCompletionFlag() => AddCompletionFlag(DEFAULT_COMPLETION_FLAG);
        public void AddCompletionFlag(string flag) => AddCondition(CustomFlagCondition.Create(flag));


        public void CompleteFlag() => CompleteFlag(DEFAULT_COMPLETION_FLAG);

        public void CompleteFlag(string flag)
        {
            AchievementCondition condition = conditions.Find(c => c.Name.Equals(flag, StringComparison.CurrentCultureIgnoreCase));

            if (condition == null)
            {
                Debug.WriteLine(new StackTrace().GetFirstDifferentAssembly().FullName);

                WebmilioCommonsMod.Instance.Logger.Warn($"Mod X tried completing condition flag `{flag}` for achievement `{Name}`.");
                return;
            }

            condition.Complete();
        }

        /// <summary>Completes the default completion flag if the given player is the local player.</summary>
        public void CompleteFlag(Player player) => CompleteFlag(player, DEFAULT_COMPLETION_FLAG);

        /// <summary>Completes the specified flag if the given player is the local player.</summary>
        /// <param name="player"></param>
        /// <param name="flag"></param>
        public void CompleteFlag(Player player, string flag)
        {
            if (player.IsLocalPlayer())
                CompleteFlag(flag);
        }


        public void AddTracker()
        {
            
        }


        public static void CompleteFlag<T>() where T : ModAchievement
        {
            if (Main.netMode == NetmodeID.Server)
                return;

            ModAchievementHelper.GetModAchievement<T>().CompleteFlag();
        }

        public static void CompleteFlag<T>(Player player) where T : ModAchievement
        {
            if (Main.netMode == NetmodeID.Server)
                return; 
            
            ModAchievementHelper.GetModAchievement<T>().CompleteFlag(player);
        }

        public static void CompleteFlag<T>(string flag) where T : ModAchievement
        {
            if (Main.netMode == NetmodeID.Server)
                return;

            ModAchievementHelper.GetModAchievement<T>().CompleteFlag(flag);
        }

        public static void CompleteFlag<T>(Player player, string flag) where T : ModAchievement
        {
            if (Main.netMode == NetmodeID.Server)
                return;

            ModAchievementHelper.GetModAchievement<T>().CompleteFlag(player, flag);
        }

        public static void CompleteFlagFor(string name)
        {
            if (Main.netMode == NetmodeID.Server)
                return;

            ModAchievementHelper.GetModAchievement(name).CompleteFlag();
        }

        public static void CompleteFlagFor(Player player, string name)
        {
            if (Main.netMode == NetmodeID.Server)
                return;

            ModAchievementHelper.GetModAchievement(name).CompleteFlag(player);
        }

        public static void CompleteFlagFor(string name, string flag)
        {
            if (Main.netMode == NetmodeID.Server)
                return;

            ModAchievementHelper.GetModAchievement(name).CompleteFlag(flag);
        }

        public static void CompleteFlagFor(string name, Player player, string flag)
        {
            if (Main.netMode == NetmodeID.Server)
                return;

            ModAchievementHelper.GetModAchievement(name).CompleteFlag(player, flag);
        }


        public string Name { get; }
        public string Description { get; }

        public AchievementCategory Category { get; }

        public virtual string TexturePath { get; }

        public virtual bool Autoload { get; protected set; } = true;

        public Mod Mod { get; internal set; }
        public Achievement GameAchievement { get; internal set; }
    }
}