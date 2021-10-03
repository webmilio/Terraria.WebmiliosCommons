﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using WebmilioCommons.Effects.Shaders.Entities.ForceField;

namespace WebmilioCommons.Effects.Shaders.ForceField
{
    public abstract class StandardForceFieldEffect : ForceFieldEffect
    {
        protected StandardForceFieldEffect(int radius, Color color, Asset<Texture2D> texture)
        {
            Radius = radius;
            Color = color;
            Texture = texture;
        }


        protected override int GetSphereWidth(Entity entity, Vector2 preOffset) => Radius;

        protected override Asset<Texture2D> GetTexture(Entity entity, Vector2 position) => Texture;

        protected override Color GetColor(Entity entity, Vector2 position) => Color;


        public int Radius { get; }

        public Color Color { get; }

        public Asset<Texture2D> Texture { get; }
    }
}