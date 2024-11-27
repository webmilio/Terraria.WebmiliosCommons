using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria.Localization;
using Terraria.UI;
using Terraria.UI.Chat;

namespace WebCom.UI;

public class UIText2 : UIElement
{
    private object _text = "";
    private float _textScale = 1f;
    private Vector2 _textSize = Vector2.Zero;
    private Color _color = Color.White;
    private Color _shadowColor = Color.Black;
    private bool _isWrapped;
    private string _visibleText;
    private string _lastTextReference;

    public DynamicSpriteFont font;
    public bool DynamicallyScaleDownToWidth;

    public string Text => _text.ToString();

    public float TextOriginX { get; set; }

    public float TextOriginY { get; set; }

    public float WrappedTextBottomPadding { get; set; }

    public bool IsWrapped
    {
        get
        {
            return _isWrapped;
        }
        set
        {
            _isWrapped = value;
            if (value)
                MinWidth.Set(0, 0); // TML: IsWrapped when true should prevent changing MinWidth, otherwise can't shrink in width due to CreateWrappedText+GetInnerDimensions logic. IsWrapped is false in ctor, so need to undo changes.
            InternalSetText(_text, _textScale);
        }
    }

    public Color TextColor
    {
        get
        {
            return _color;
        }
        set
        {
            _color = value;
        }
    }

    public Color ShadowColor
    {
        get
        {
            return _shadowColor;
        }
        set
        {
            _shadowColor = value;
        }
    }

    public event Action OnInternalTextChange;

    public UIText2(DynamicSpriteFont font, string text, float textScale = 1f)
    {
        this.font = font;

        TextOriginX = 0.5f;
        TextOriginY = 0f;
        IsWrapped = false;
        WrappedTextBottomPadding = 20f;

        InternalSetText(text, textScale);
    }

    public UIText2(DynamicSpriteFont font, LocalizedText text, float textScale = 1f)
    {
        this.font = font;

        TextOriginX = 0.5f;
        TextOriginY = 0f;
        IsWrapped = false;
        WrappedTextBottomPadding = 20f;

        InternalSetText(text, textScale);
    }

    public override void Recalculate()
    {
        InternalSetText(_text, _textScale);
        base.Recalculate();
    }

    public void SetText(string text)
    {
        InternalSetText(text, _textScale);
    }

    public void SetText(LocalizedText text)
    {
        InternalSetText(text, _textScale);
    }

    public void SetText(LocalizedText text, float textScale)
    {
        InternalSetText(text, textScale);
    }

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        base.DrawSelf(spriteBatch);
        VerifyTextState();
        CalculatedStyle innerDimensions = GetInnerDimensions();

        Vector2 position = innerDimensions.Position();
        position.Y -= 2f * _textScale;

        position.X += (innerDimensions.Width - _textSize.X) * TextOriginX;
        position.Y += (innerDimensions.Height - _textSize.Y) * TextOriginY;
        float num = _textScale;
        if (DynamicallyScaleDownToWidth && _textSize.X > innerDimensions.Width)
            num *= innerDimensions.Width / _textSize.X;

        Vector2 vector = font.MeasureString(_visibleText);
        Color baseColor = _shadowColor * ((float)(int)_color.A / 255f);
        Vector2 origin = new Vector2(0f, 0f) * vector;
        Vector2 baseScale = new Vector2(num);
        TextSnippet[] snippets = ChatManager.ParseMessage(_visibleText, _color).ToArray();
        ChatManager.ConvertNormalSnippets(snippets);
        ChatManager.DrawColorCodedStringShadow(spriteBatch, font, snippets, position, baseColor, 0f, origin, baseScale, -1f, 1.5f);
        ChatManager.DrawColorCodedString(spriteBatch, font, snippets, position, Color.White, 0f, origin, baseScale, out var _, -1f);
    }

    private void VerifyTextState()
    {
        if ((object)_lastTextReference != Text)
            InternalSetText(_text, _textScale);
    }

    private void InternalSetText(object text, float textScale)
    {
        _text = text;
        _textScale = textScale;
        _lastTextReference = _text.ToString();
        if (IsWrapped)
            _visibleText = font.CreateWrappedText(_lastTextReference, GetInnerDimensions().Width / _textScale);
        else
            _visibleText = _lastTextReference;

        // TML: Changed to use ChatManager.GetStringSize() since using DynamicSpriteFont.MeasureString() ignores chat tags,
        // giving the UI element a much larger calculated size than it should have.
        Vector2 vector = ChatManager.GetStringSize(font, _visibleText, new Vector2(1));

        Vector2 vector2 = (_textSize = ((!IsWrapped) ? (new Vector2(vector.X, 16f) * textScale) : (new Vector2(vector.X, vector.Y + WrappedTextBottomPadding) * textScale)));
        if (!IsWrapped)
        { // TML: IsWrapped when true should prevent changing MinWidth, otherwise can't shrink in width due to logic.
            MinWidth.Set(vector2.X + PaddingLeft + PaddingRight, 0f);
        }
        MinHeight.Set(vector2.Y + PaddingTop + PaddingBottom, 0f);
        if (this.OnInternalTextChange != null)
            this.OnInternalTextChange();
    }
}
