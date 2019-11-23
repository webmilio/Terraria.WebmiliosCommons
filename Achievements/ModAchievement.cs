using System;
using System.Collections.Generic;
using Terraria.Achievements;
using Terraria.ModLoader;
using WebmilioCommons.Extensions;

namespace WebmilioCommons.Achievements
{
    public class ModAchievement
    {
        internal readonly List<AchievementCondition> conditions = new List<AchievementCondition>();


        public ModAchievement(string name, string description, AchievementCategory category)
        {
            Name = name;
            Description = description;

            Category = category;

            Type type = GetType();
            TexturePath = $"{type.GetModFromType().Name}/{type.GetPath()}";
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


        public void AddTracker()
        {

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