using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Shaders;

namespace WebmilioCommons.Effects.Shaders.Entities.FullGlow
{
    public class FullGlowEffect : EntityMiscShaderEffect
    {
        protected MiscShaderData shaderData;
        protected readonly string miscDictionaryKey;


        public FullGlowEffect()
        {
            miscDictionaryKey = $"{GetType().FullName}";

            if (shaderData == default)
            {
                shaderData = new MiscShaderData(new Ref<Effect>(WebmilioCommonsMod.Instance.Assets.Request<Effect>("Effects/FullGlow/FullGlow").Value), "FullGlow");
                GameShaders.Misc[miscDictionaryKey] = shaderData;
            }

            GameShaders.Misc[miscDictionaryKey] = shaderData;
        }


        public override void Apply(SpriteBatch spriteBatch, Entity entity)
        {
            BeginStandardSpriteBatch(spriteBatch, BlendState.NonPremultiplied);

            shaderData.Apply();

            RestoreSpriteBatch(spriteBatch);
        }
    }
}