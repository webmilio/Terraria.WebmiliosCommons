using System;
using Terraria.ModLoader;
using WebmilioCommons.Achievements.Helper;
using WebmilioCommons.Loaders;

namespace WebmilioCommons.Achievements
{
    public class ModAchievementLoader : SingletonLoader<ModAchievementLoader, ModAchievement>
    {
        protected override void PostAdd(Mod mod, ModAchievement item, Type type)
        {
            if (!item.Autoload)
                return;

            item.Mod = mod;
            ModAchievementHelper.RegisterAchievement(item, type);
        }

        public override void PostLoad()
        {
            foreach (var modAchievement in Generics)
            {
                var texture = modAchievement.Texture;
            }
        }
    }
}