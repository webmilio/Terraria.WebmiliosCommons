using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameInput;
using Terraria.UI;

namespace WebmilioCommons.UI;

public class UITextBox : Terraria.GameContent.UI.Elements.UITextBox
{
    public delegate void UITextBoxDelegate(UITextBox element);

    public UITextBox() : this(int.MaxValue) { }
    public UITextBox(int maxLength) : this("", maxLength: maxLength) { }

    public UITextBox(string text, float textScale = 1, bool large = true, int maxLength = int.MaxValue) : base(text, textScale, large)
    {
        SetTextMaxLength(maxLength);
    }

    public override void Click(UIMouseEvent evt)
    {
        base.Click(evt);
        Focus();
    }

    public override void Update(GameTime gameTime)
    {
        if (Focused)
        {
            if (Main.mouseLeft && !ContainsPoint(Main.MouseScreen))
                Unfocus();
        }

        base.Update(gameTime);
    }

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        if (Focused)
        {
            PlayerInput.WritingText = true;
            Main.instance.HandleIME();

            var newText = Main.GetInputText(Text);

            if (newText != Text)
            {
                SetText(newText);
                TextChanged?.Invoke(this);
            }
        }

        base.DrawSelf(spriteBatch);
    }

    #region Focus

    public void Focus()
    {
        if (Focused) return;
        Focused = true;

        Main.clrInput();
        Main.blockInput = true;
        ShowInputTicker = true;

        GainFocus?.Invoke(this);
    }

    public void Unfocus()
    {
        if (!Focused) return;
        Focused = false;

        Main.blockInput = false;
        ShowInputTicker = false;

        LoseFocus?.Invoke(this);
    }

    public bool Focused { get; private set; }

    public event UITextBoxDelegate GainFocus, LoseFocus;

    #endregion

    public event UITextBoxDelegate TextChanged;
}