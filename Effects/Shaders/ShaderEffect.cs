using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using WebmilioCommons.Extensions;
using WebmilioCommons.Loaders;

namespace WebmilioCommons.Effects.Shaders
{
    public abstract class ShaderEffect : IAssociatedToMod
    {
        private static string _shaderRoot;


        protected ShaderEffect()
        {
        }


        public abstract void Apply(SpriteBatch spriteBatch, Vector2 positionPreOffset);


        internal static void RestoreSpriteBatch(SpriteBatch spriteBatch)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.ZoomMatrix);
        }


        public virtual bool Unique { get; } = false;


        public Mod Mod { get; set; }
    }
}