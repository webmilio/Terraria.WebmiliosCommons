using System;
using Terraria;
using Terraria.ModLoader;
using WebmilioCommons.Commons.Players;
using WebmilioCommons.Players;
using WebmilioCommons.Systems;

namespace WebmilioCommons.Transformations
{
    public abstract class ModPlayerTransformation<T> : PlayerTransformation, IModPlayerLinked<T> where T : ModPlayer
    {
        private T _modPlayer;


        public T ModPlayer
        {
            get
            {
                if (_modPlayer == default)
                    _modPlayer = ModPlayerGetter(Player);

                return _modPlayer;
            }
        }

        public virtual Func<Player, T> ModPlayerGetter { get; } = player => player.GetModPlayer<T>();
    }
}