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
    /// <summary>This class serves as the public surface linking the vanilla achievement system and the modded achievement system together.</summary>
    public class ModAchievement
    {
        /// <summary>The name of the default completion flag.</summary>
        public const string DEFAULT_COMPLETION_FLAG = "Completed";
        internal readonly List<AchievementCondition> conditions = new List<AchievementCondition>();


        /// <summary></summary>
        /// <param name="name">The displayed name for the achievement.</param>
        /// <param name="description">The displayed description for the achievement.</param>
        /// <param name="category">The vanilla category under to which this achievement belongs.</param>
        public ModAchievement(string name, string description, AchievementCategory category)
        {
            Name = name;
            Description = description;

            Category = category;

            Type type = GetType();
            TexturePath = $"{type.GetModFromType().Name}/{type.GetPath()}";
        }


        /// <summary>Called after the current object has been instantiated. You want to add your achievement conditions here.</summary>
        public virtual void SetDefaults() { }


        /// <summary>Add a condition for achievement completion.</summary>
        /// <param name="condition">The condition to add.</param>
        public void AddCondition(AchievementCondition condition)
        {
            conditions.Add(condition);
        }

        /// <summary>Add multiple conditions for achievement completion.</summary>
        /// <param name="conditions">The conditions to add.</param>
        public void AddConditions(params AchievementCondition[] conditions)
        {
            for (int i = 0; i < conditions.Length; i++)
                AddCondition(conditions[i]);
        }


        /// <summary>Adds a default completion flag to be manually triggered with <see cref="CompleteFlag()"/>.</summary>
        public void AddCompletionFlag() => AddCompletionFlag(DEFAULT_COMPLETION_FLAG);

        /// <summary>Adds a named completion flag to be manually triggered with <see cref="CompleteFlag()"/>.</summary>
        /// <param name="flag">The name of the flag.</param>
        public void AddCompletionFlag(string flag) => AddCondition(CustomFlagCondition.Create(flag));


        /// <summary>Completes the default completion flag.</summary>
        public void CompleteFlag() => CompleteFlag(DEFAULT_COMPLETION_FLAG);
        
        /// <summary>Completes the specified named completion flag.</summary>
        /// <param name="flag">The name of the completion flag.</param>
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


        /// <summary>Adds a tracker to the achievement, requiring a certain amount of tasks to be completed.</summary>
        [Obsolete("Trackers are not yet implemented.")]
        public void AddTracker()
        {
            
        }


        /// <summary>Completes the default completion flag for the specified type.</summary>
        /// <typeparam name="T">The type of the <see cref="ModAchievement"/> to complete.</typeparam>
        public static void CompleteFlag<T>() where T : ModAchievement
        {
            if (Main.netMode == NetmodeID.Server)
                return;

            ModAchievementHelper.GetModAchievement<T>().CompleteFlag();
        }

        /// <summary>Completes the default completion flag of the specified type for the specified player.</summary>
        /// <typeparam name="T">The type of the <see cref="ModAchievement"/> to complete.</typeparam>
        /// <param name="player">The player for whom to complete said flag.</param>
        public static void CompleteFlag<T>(Player player) where T : ModAchievement
        {
            if (Main.netMode == NetmodeID.Server)
                return; 
            
            ModAchievementHelper.GetModAchievement<T>().CompleteFlag(player);
        }

        /// <summary>Completes the given completion flag for the specified type.</summary>
        /// <typeparam name="T">The type of the <see cref="ModAchievement"/> to complete.</typeparam>
        /// <param name="flag">The name of the completion flag.</param>
        public static void CompleteFlag<T>(string flag) where T : ModAchievement
        {
            if (Main.netMode == NetmodeID.Server)
                return;

            ModAchievementHelper.GetModAchievement<T>().CompleteFlag(flag);
        }

        /// <summary>Completes the given completion flag of the specified type for the given player.</summary>
        /// <typeparam name="T">The type of the <see cref="ModAchievement"/> to complete.</typeparam>
        /// <param name="player">The player for whom to complete said flag.</param>
        /// <param name="flag">The name of the completion flag.</param>
        public static void CompleteFlag<T>(Player player, string flag) where T : ModAchievement
        {
            if (Main.netMode == NetmodeID.Server)
                return;

            ModAchievementHelper.GetModAchievement<T>().CompleteFlag(player, flag);
        }

        /// <summary>Completes the default completion flag for the given achievement.</summary>
        /// <param name="name">The name of the achievement.</param>
        public static void CompleteFlagFor(string name)
        {
            if (Main.netMode == NetmodeID.Server)
                return;

            ModAchievementHelper.GetModAchievement(name).CompleteFlag();
        }

        /// <summary>Completes the default completion flag of the given achievement for the given player.</summary>
        /// <param name="player">The player for whom to complete said flag.</param>
        /// <param name="name">The name of the achievement.</param>
        public static void CompleteFlagFor(Player player, string name)
        {
            if (Main.netMode == NetmodeID.Server)
                return;

            ModAchievementHelper.GetModAchievement(name).CompleteFlag(player);
        }

        /// <summary>Completes the default completion flag of the given achievement for the given player.</summary>
        /// <param name="name">The name of the achievement.</param>
        /// <param name="flag">The name of the completion flag.</param>
        public static void CompleteFlagFor(string name, string flag)
        {
            if (Main.netMode == NetmodeID.Server)
                return;

            ModAchievementHelper.GetModAchievement(name).CompleteFlag(flag);
        }

        /// <summary>Completes the specified completion flag of the given achievement for the given player.</summary>
        /// <param name="name">The name of the achievement.</param>
        /// <param name="player">The player for whom to complete said flag.</param>
        /// <param name="flag">The name of the completion flag.</param>
        public static void CompleteFlagFor(string name, Player player, string flag)
        {
            if (Main.netMode == NetmodeID.Server)
                return;

            ModAchievementHelper.GetModAchievement(name).CompleteFlag(player, flag);
        }


        /// <summary>The displayed name for the achievement.</summary>
        public string Name { get; }

        /// <summary>The displayed description for the achievement.</summary>
        public string Description { get; }

        /// <summary>The vanilla category under which the achievement falls.</summary>
        public AchievementCategory Category { get; }

        /// <summary>The path to the texture for the achievement.</summary>
        public virtual string TexturePath { get; }

        /// <summary>true if the achievement be automatically created upon loading all <see cref="ModAchievement"/>.</summary>
        public virtual bool Autoload { get; protected set; } = true;

        /// <summary>The mod from which this <see cref="ModAchievement"/> originates.</summary>
        public Mod Mod { get; internal set; }

        /// <summary>The vanilla category under to which this achievement belongs.</summary>
        public Achievement GameAchievement { get; internal set; }
    }
}