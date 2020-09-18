using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace WebmilioCommons.Effects.Shaders
{
    public abstract class EntityMiscShaderEffect : ShaderEffect
    {
        public abstract void Apply(SpriteBatch spriteBatch, Entity entity);
    }
}