using System;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameInput;
using Terraria.UI;

namespace WebmilioCommons.UI.Slots;

// Based entirely on ExampleMod.
public class ItemSlot : UIElement
{
    public ItemSlot(int context = Terraria.UI.ItemSlot.Context.BankItem, float scale = 1f)
    {
        Context = context;
        Scale = scale;

        Item = new Item();
        Item.SetDefaults();

        Width.Set(52 * scale, 0f);
        Height.Set(52 * scale, 0f);
    }


    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        float oldScale = Main.inventoryScale;
        Main.inventoryScale = Scale;

        var rect = GetDimensions().ToRectangle();
        var item = Item;

        if (ContainsPoint(Main.MouseScreen) && !PlayerInput.IgnoreMouseInterface)
        {
            Main.LocalPlayer.mouseInterface = true;

            if (ValidItemFunction == null || ValidItemFunction(Main.mouseItem, Main.LocalPlayer))
            {
                var old = item;
                Terraria.UI.ItemSlot.Handle(ref item, Context);

                Item = item;
                ChangeItem(old, item);
            }
        }

        Terraria.UI.ItemSlot.Draw(spriteBatch, ref item, Context, rect.TopLeft());
        Main.inventoryScale = oldScale;

        base.DrawSelf(spriteBatch);
    }

    public virtual void ChangeItem(Item old, Item @new)
    {

    }


    public Func<Item, Player, bool> ValidItemFunction { get; set; }


    public int Context { get; }
    public float Scale { get; }

    public Item Item { get; private set; }
}