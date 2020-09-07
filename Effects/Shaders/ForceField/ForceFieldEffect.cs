using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;
using WebmilioCommons.Extensions;
using WebmilioCommons.Loaders;

namespace WebmilioCommons.Effects.Shaders.ForceField
{
    public abstract class ForceFieldEffect : ShaderEffect
    {
        private const string VANILLA_MISC_BASE = "Images/Misc/";

        public const string
            PERLIN = VANILLA_MISC_BASE + "Perlin",
            NOISE = "noise",
            MISC_SHADER_NAME = "ForceField";


        private static Texture2D _streaks, _streaks2;


        private MiscShaderData _shaderData;
        private readonly string _miscDictionaryKey;


        protected ForceFieldEffect()
        {
            string key = $"{GetType().FullName}";

            if (Unique)
                key += $".{GetHashCode()}";

            _miscDictionaryKey = key;
        }


        public override void Apply(SpriteBatch spriteBatch, Vector2 positionPreOffset)
        {
            BeginForceFieldSpriteBatch(spriteBatch);

            Entity entity = default;

            int
                width = GetSphereWidth(entity, positionPreOffset),
                height = GetSphereHeight(entity, positionPreOffset, width);

            if (_shaderData == default)
            {
                _shaderData = new MiscShaderData(Main.PixelShaderRef, MISC_SHADER_NAME);
                GameShaders.Misc[_miscDictionaryKey] = _shaderData;
            }

            var position = positionPreOffset - GetSphereOffset(entity, positionPreOffset, width, height);

            var drawData = new DrawData(GetTexture(entity, position),
                position, GetSphereBoundingBox(entity, position, width, height),
                Color.White, GetRotation(entity, position),
                Vector2.Zero, GetScale(entity, position), GetSpriteEffects(entity, position), 0);

            _shaderData.UseColor(GetColor(entity, position));
            _shaderData.Apply(drawData);
            drawData.Draw(spriteBatch);

            RestoreSpriteBatch(spriteBatch);
        }


        protected void BeginForceFieldSpriteBatch(SpriteBatch spriteBatch)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.ZoomMatrix);
        }


        protected abstract int GetSphereWidth(Entity entity, Vector2 targetPositionPreOffset);
        protected virtual int GetSphereHeight(Entity entity, Vector2 targetPositionPreOffset, int width) => width * 2 / 3;

        protected virtual Vector2 GetSphereOffset(Entity entity, Vector2 targetPositionPreOffset, int width, int height) => new Vector2(width / 2, height / 2);

        protected virtual string GetTexturePath(Entity entity, Vector2 position) => NOISE;
        protected virtual Texture2D GetTexture(Entity entity, Vector2 position) => TextureManager.Load(GetTexturePath(entity, position));

        protected virtual Rectangle GetSphereBoundingBox(Entity entity, Vector2 position, int width, int height) => new Rectangle(0, 0, width, height);

        protected virtual float GetRotation(Entity entity, Vector2 position) => 0f;
        protected virtual float GetScale(Entity entity, Vector2 position) => 1f;

        protected virtual SpriteEffects GetSpriteEffects(Entity entity, Vector2 position) => SpriteEffects.None;

        protected virtual Color GetColor(Entity entity, Vector2 position) => Color.White;


        public static Texture2D StreaksTexture => _streaks ?? (_streaks = WebmilioCommonsMod.Instance.GetTexture($"{typeof(ForceFieldEffect).GetRootPath()}/streaks"));
        public static Texture2D Streaks2Texture => _streaks2 ?? (_streaks2 = WebmilioCommonsMod.Instance.GetTexture($"{typeof(ForceFieldEffect).GetRootPath()}/streaks2"));
    }
}