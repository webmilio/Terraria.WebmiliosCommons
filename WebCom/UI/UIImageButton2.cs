using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ReLogic.Content;
using Terraria.UI;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace WebCom.UI;

public class UIImageButton2 : UIElement
{
    private Asset<Texture2D> _texture;

    private float _visibilityActive = 1f;
    private float _visibilityInactive = 0.4f;

    private Asset<Texture2D> _borderTexture;

    public float Rotation { get; set; }

    public UIImageButton2(Asset<Texture2D> texture)
    {
        _texture = texture;
        Width.Set(_texture.Width(), 0f);
        Height.Set(_texture.Height(), 0f);
    }

    public void SetHoverImage(Asset<Texture2D> texture)
    {
        _borderTexture = texture;
    }

    public void SetImage(Asset<Texture2D> texture)
    {
        _texture = texture;

        Width.Set(_texture.Width(), 0f);
        Height.Set(_texture.Height(), 0f);
    }

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        var dimensions = GetDimensions();
        spriteBatch.Draw(_texture.Value, dimensions.Position(), Color.White * (IsMouseHovering ? _visibilityActive : _visibilityInactive));

        if (_borderTexture != null && IsMouseHovering)
        {
            spriteBatch.Draw(_borderTexture.Value, dimensions.Position(), Color.White);
        }
    }

    public override void MouseOver(UIMouseEvent evt)
    {
        base.MouseOver(evt);

        SoundEngine.PlaySound(SoundID.MenuTick);
    }

    public void SetVisibility(float whenActive, float whenInactive)
    {
        _visibilityActive = MathHelper.Clamp(whenActive, 0f, 1f);
        _visibilityInactive = MathHelper.Clamp(whenInactive, 0f, 1f);
    }
}
