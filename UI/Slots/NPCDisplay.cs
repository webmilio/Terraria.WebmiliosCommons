using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent;
using Terraria.UI;
using WebmilioCommons.Extensions;

namespace WebmilioCommons.UI.Slots;

public class NPCDisplay : UIElement
{
    private Rectangle _sourceRectangle;
    private int _frameCounter, _frameTimer;

    public NPCDisplay() { }

    public NPCDisplay(int npcType, float scale = 1f)
    {
        SetNPC(npcType, scale);
    }

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        if (NPC == null)
        {
            base.DrawSelf(spriteBatch);
            return;
        }

        if (_frameTimer == 0)
        {
            var maxFrame = Main.npcFrameCount[NPC.type];
            var frameHeight = Texture.Value.Height / maxFrame;

            _sourceRectangle = new(0, frameHeight * _frameCounter, Texture.Value.Width, frameHeight);
            _frameCounter++;

            if (_frameCounter == maxFrame)
                _frameCounter = 0;
        }

        var dimensions = GetDimensions();
        var drawPosition = dimensions.Center() + new Vector2(0, -2);

        float scale = MathF.Min(dimensions.Width / _sourceRectangle.Width, dimensions.Height / _sourceRectangle.Height);

        if (scale > 1)
            scale = 1;

        spriteBatch.Draw(Texture.Value, drawPosition, _sourceRectangle, 
            NPC.color == default ? Color.White : NPC.color, 0, _sourceRectangle.CenterSize(), 
            scale, SpriteEffects.None, 0);

        _frameTimer = (_frameTimer + 1) % TicksPerFrame;
        base.DrawSelf(spriteBatch);
    }

    public void SetNPC(int npcType, float scale = 1)
    {
        _frameCounter = _frameTimer = 0;

        NPC = new();
        NPC.SetDefaults(npcType);

        Texture = TextureAssets.Npc.GetAndLoad(npcType);
        Scale = scale;
    }

    public NPC NPC { get; protected set; }

    public float Scale { get; protected set; }
    public Asset<Texture2D> Texture { get; protected set; }

    public int TicksPerFrame { get; set; } = Constants.TicksPerSecond / 3;
}