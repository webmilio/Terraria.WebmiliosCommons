using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace WebmilioCommons.Effects.Shaders.Entities
{
    public abstract class EntityMiscShaderEffect : ShaderEffect
    {
        public abstract void Apply(SpriteBatch spriteBatch, Entity entity);


        protected void BeginStandardSpriteBatch(SpriteBatch spriteBatch, BlendState blendState)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, blendState, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.ZoomMatrix);
        }
    }
}