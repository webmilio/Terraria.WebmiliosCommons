using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria.ModLoader;

namespace WebmilioCommons.Players
{
    public abstract class BetterModPlayer : ModPlayer
    {
        private static readonly List<Type> _types = new List<Type>();

        protected BetterModPlayer()
        {
            var type = GetType();

            if (!_types.Contains(type))
                return;

            _types.Add(type);
        }


        public static bool IsInitialized<T>() => _types.Contains(typeof(T));
        public static bool IsInitialized(Type type) => _types.Contains(type);





        public virtual bool CanInteractWithTownNPCs() => true;
    }
}