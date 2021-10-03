using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;
using WebmilioCommons.Extensions;

namespace WebmilioCommons.Effects.Shaders.Entities.ForceField
{
    public abstract class ForceFieldEffect : EntityMiscShaderEffect
    {
        private MiscShaderData _shaderData;
        private readonly string _miscDictionaryKey;


        protected ForceFieldEffect()
        {
            string key = $"{GetType().FullName}";

            if (Unique)
                key += $".{GetHashCode()}";

            _miscDictionaryKey = key;
        }

        ~ForceFieldEffect()
        {
            GameShaders.Misc.Remove(_miscDictionaryKey);
        }


        public virtual bool PreDraw(SpriteBatch spriteBatch, Entity entity, ref Vector2 position) => true;

        public override void Apply(SpriteBatch spriteBatch, Entity entity)
        {
            var positionPreOffset = entity.ScreenPosition();
            int
                width = GetSphereWidth(entity, positionPreOffset),
                height = GetSphereHeight(entity, positionPreOffset, width);

            BeginStandardSpriteBatch(spriteBatch, GetBlendState(entity, positionPreOffset));

            if (_shaderData == default)
            {
                _shaderData = new MiscShaderData(Main.PixelShaderRef, MiscShaderName);
                GameShaders.Misc[_miscDictionaryKey] = _shaderData;
            }

            var position = positionPreOffset - GetSphereOffset(entity, positionPreOffset, width, height);

            var drawData = new DrawData(GetTexture(entity, position).Value,
                position, GetSphereBoundingBox(entity, position, width, height),
                Color.White, GetRotation(entity, position),
                Vector2.Zero, GetScale(entity, position), GetSpriteEffects(entity, position), 0);

            _shaderData.UseColor(GetColor(entity, position).MultiplyRGBA(new Color(1f, 1f, 1f, 0f)));
            _shaderData.Apply(drawData);

            if (PreDraw(spriteBatch, entity, ref position))
            {
                drawData.Draw(spriteBatch);
                PostDraw(spriteBatch, entity, position);
            }

            RestoreSpriteBatch(spriteBatch);
        }

        public virtual void PostDraw(SpriteBatch spriteBatch, Entity entity, Vector2 position) { }



        protected virtual BlendState GetBlendState(Entity entity, Vector2 preOffset) => BlendState.NonPremultiplied;

        protected abstract int GetSphereWidth(Entity entity, Vector2 preOffset);
        protected virtual int GetSphereHeight(Entity entity, Vector2 preOffset, int width) => width * 2 / 3;

        protected virtual Vector2 GetSphereOffset(Entity entity, Vector2 preOffset, int width, int height) => new Vector2(width / 2, height / 2);

        protected virtual string GetTexturePath(Entity entity, Vector2 position) => NOISE;
        protected virtual Asset<Texture2D> GetTexture(Entity entity, Vector2 position) => ModContent.Request<Texture2D>(GetTexturePath(entity, position));

        protected virtual Rectangle GetSphereBoundingBox(Entity entity, Vector2 position, int width, int height) => new Rectangle(0, 0, width, height);

        protected virtual float GetRotation(Entity entity, Vector2 position) => 0f;
        protected virtual float GetScale(Entity entity, Vector2 position) => 1f;

        protected virtual SpriteEffects GetSpriteEffects(Entity entity, Vector2 position) => SpriteEffects.None;

        protected virtual Color GetColor(Entity entity, Vector2 position) => Color.White;
    }
}