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
        private const string VANILLA_MISC_BASE = "Images/Misc/";

        public const string
            PERLIN = VANILLA_MISC_BASE + "Perlin",
            NOISE = VANILLA_MISC_BASE + "noise",
            MiscShaderName = "ForceField";


        private static Texture2D _streaks, _streaks2;



        protected ShaderEffect()
        {
            
        }


        public virtual void Unload()
        {

        }

        internal static void RestoreSpriteBatch(SpriteBatch spriteBatch)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.ZoomMatrix);
        }


        public virtual bool Unique { get; } = false;


        public Mod Mod { get; set; }

        public static Texture2D StreaksTexture => _streaks ?? (_streaks = WebmilioCommonsMod.Instance.GetTexture($"{typeof(ShaderEffect).GetRootPath()}/streaks"));
        public static Texture2D Streaks2Texture => _streaks2 ?? (_streaks2 = WebmilioCommonsMod.Instance.GetTexture($"{typeof(ShaderEffect).GetRootPath()}/streaks2"));
    }
}