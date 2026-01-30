using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI;

namespace WebCom.UI;

public class DraggableContainer : UIElement
{
  private bool _dragging = false;
  private Vector2 _dragOffset;

  public DraggableContainer(UIElement element)
  {
    Append(element);
  }

  protected override void DrawSelf(SpriteBatch spriteBatch)
  {
    base.DrawSelf(spriteBatch);

    // spriteBatch.Draw(TextureAssets.MagicPixel.Value, GetDimensions().ToRectangle(), Color.Red);

    if (ContainsPoint(Main.MouseScreen))
    {
      Main.LocalPlayer.mouseInterface = true;
    }

    if (_dragging)
    {
      var position = Main.MouseScreen - _dragOffset;

      Left.Set(position.X, 0);
      Top.Set(position.Y, 0);

      HAlign = 0;
      VAlign = 0;

      Recalculate();
    }
  }

  public override void RightMouseDown(UIMouseEvent evt)
  {
    base.RightMouseDown(evt);
    _dragging = true;

    var dimensions = GetDimensions();
    _dragOffset = Main.MouseScreen - dimensions.Position();
  }

  public override void RightMouseUp(UIMouseEvent evt)
  {
    _dragging = false;
    _dragOffset = Vector2.Zero;

    base.RightMouseUp(evt);
  }

  public static DraggableContainer Fill(UIElement element)
  {
    return new(element)
    {
      Width = StyleDimension.Fill,
      Height = StyleDimension.Fill,
    };
  }
}

public class DraggableContainer<T>(T element) : DraggableContainer(element) where T : UIElement
{
  public T Child { get; }

  public static DraggableContainer<T> Fill(T element)
  {
    return new(element)
    {
      Width = StyleDimension.Fill,
      Height = StyleDimension.Fill
    };
  }
}