using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.UI;

namespace WebCom.UI;

public class UITextBox2 : UITextBox
{
  private bool _editing;

  public UITextBox2(string text, float textScale = 1, bool large = false) : base(text, textScale, large)
  {
    OnLeftClick += TextBox_OnLeftClick;
  }

  ~UITextBox2()
  {
    OnLeftClick -= TextBox_OnLeftClick;
  }

  public override void Update(GameTime gameTime)
  {
    if (_editing)
    {
      PlayerInput.WritingText = true;
      Main.CurrentInputTextTakerOverride = this;

      if (Main.mouseLeft && !ContainsPoint(new Vector2(Main.mouseX, Main.mouseY)))
      {
        StopEditing();
      }
    }

    base.Update(gameTime);
  }

  protected override void DrawSelf(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
  {
    if (_editing)
    {
      PlayerInput.WritingText = true;
      Main.instance.HandleIME();

      SetText(Main.GetInputText(Text));

      if (Main.inputTextEnter || Main.inputTextEscape)
      {
        StopEditing();
      }
    }

    ShowInputTicker = _editing;

    base.DrawSelf(spriteBatch);
  }

  private void TextBox_OnLeftClick(UIMouseEvent evt, UIElement listeningElement)
  {
    Main.clrInput();
    _editing = true;
  }

  private void StopEditing()
  {
    _editing = false;
    Main.clrInput();
  }
}
