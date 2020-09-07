using System;
using Terraria.ModLoader;
using WebmilioCommons.Loaders;

namespace WebmilioCommons.Effects.Shaders
{
    public sealed class ShaderEffectsLoader : SingletonLoader<ShaderEffectsLoader, ShaderEffect>
    {
        protected override void PostAdd(Mod mod, ShaderEffect item, Type type)
        {
            item.Mod = mod;
        }
    }
}