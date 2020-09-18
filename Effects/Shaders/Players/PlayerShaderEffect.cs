using Terraria;
using WebmilioCommons.Players;

namespace WebmilioCommons.Effects.Shaders.Players
{
    public abstract class PlayerShaderEffect : ShaderEffect
    {
        public abstract void Apply(WCPlayer wcPlayer, Player player);
    }
}