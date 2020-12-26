using System;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameInput;
using Terraria.UI;

namespace WebmilioCommons.UI.Slots
{
    // Based entirely on ExampleMod.
    public class ItemSlot : UIElement
    {
        public ItemSlot(int context = Terraria.UI.ItemSlot.Context.BankItem, float scale = 1f)
        {
            Context = context;
            Scale = scale;

            Item = new Item();
            Item.SetDefaults();

            Width.Set(Main.inventoryBack9Texture.Width * scale, 0f);
            Height.Set(Main.inventoryBack9Texture.Height * scale, 0f);
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

                if (ValidItemFunction == null || ValidItemFunction(Main.mouseItem))
                {
                    Terraria.UI.ItemSlot.Handle(ref item, Context);
                    Item = item;
                }
            }

            Terraria.UI.ItemSlot.Draw(spriteBatch, ref item, Context, rect.TopLeft());
            Main.inventoryScale = oldScale;
        }


        public Func<Item, bool> ValidItemFunction { get; set; }


        public int Context { get; }
        public float Scale { get; }

        public Item Item { get; private set; }
    }
}