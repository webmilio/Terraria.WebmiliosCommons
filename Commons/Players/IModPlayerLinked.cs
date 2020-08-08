using System;
using Terraria;
using Terraria.ModLoader;

namespace WebmilioCommons.Commons.Players
{
    public interface IModPlayerLinked<T> where T : ModPlayer
    {
        T ModPlayer { get; }

        Func<Player, T> ModPlayerGetter { get; }
    }
}