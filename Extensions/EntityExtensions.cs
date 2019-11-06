using System;
using Terraria;

namespace WebmilioCommons.Extensions
{
    public static class EntityExtensions
    {
        public static void ForAllActive<T>(this T[] entities, Action<T> action) where T : Entity
        {
            for (int i = 0; i < entities.Length; i++)
                if (entities[i] != null && entities[i].active)
                    action(entities[i]);
        }
    }
}