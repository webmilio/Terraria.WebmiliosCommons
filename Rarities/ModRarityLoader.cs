using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Xna.Framework;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using Terraria;
using Terraria.GameContent.UI;
using Terraria.UI.Chat;
using WebmilioCommons.Loaders;

namespace WebmilioCommons.Rarities
{
    public sealed class ModRarityLoader : SingletonLoader<ModRarityLoader, ModRarity>
    {
        private int _highestVanillaRarity = 99;
        private int _nextRarityId;

        private MethodInfo _drawItemTooltip, _drawBuffString;

        private Dictionary<int, Color> _rarities;


        public override void PreLoad()
        {
            try
            {
                Type irType = typeof(ItemRarity);
                FieldInfo irField = irType.GetField(nameof(_rarities), BindingFlags.NonPublic | BindingFlags.Static);
                object irDic = irField.GetValue(null);

                _rarities = irDic as Dictionary<int, Color>;


                Type mainType = typeof(Main);
                _drawItemTooltip = mainType.GetMethod("MouseText_DrawItemTooltip", BindingFlags.NonPublic | BindingFlags.Instance);
                _drawBuffString = mainType.GetMethod("MouseText_DrawBuffString", BindingFlags.NonPublic | BindingFlags.Instance);


                //On.Terraria.Main.MouseText += Main_OnMouseText;
                //On.Terraria.Main.MouseText_DrawItemTooltip += Main_OnMouseText_DrawItemTooltip;
                IL.Terraria.Main.MouseText += Main_OnMouseText;
                IL.Terraria.Main.MouseText_DrawItemTooltip += Main_OnMouseText_DrawItemTooltip;

                HookingSuccessful = true;
            }
            catch (Exception e)
            {
                WebmilioCommonsMod.Instance.Logger.WarnFormat("Couldn't load custom rarities; all rarity IDs will be set to {0}.\nInner Exception: {1}",
                    nameof(ModRarity.LowerVanillaRarity), e);
            }

            _nextRarityId = _highestVanillaRarity + 1;
        }


        public override void PostLoad()
        {
            ForAllGeneric(rarity =>
            {
                if (HookingSuccessful)
                {
                    rarity.Id = _nextRarityId++;
                    _rarities.Add(rarity.Id, rarity.Color);
                }
                else
                    rarity.Id = rarity.LowerVanillaRarity;
            });
        }

        protected override void PreUnload()
        {
            ForAllGeneric(rarity => _rarities.Remove(rarity.Id));

            _rarities = null;

            //On.Terraria.Main.MouseText -= Main_OnMouseText;
            //On.Terraria.Main.MouseText_DrawItemTooltip -= Main_OnMouseText_DrawItemTooltip;

            if (HookingSuccessful)
            {
                IL.Terraria.Main.MouseText -= Main_OnMouseText;
                IL.Terraria.Main.MouseText_DrawItemTooltip -= Main_OnMouseText_DrawItemTooltip;
            }
        }


        private void Main_OnMouseText(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            // Finding baseColor index.
            int baseColorIndex = 0;
            c.GotoNext(i => i.MatchLdloca(out baseColorIndex), i => i.MatchLdsfld<Main>("mouseTextColor"));

            c.Goto(null); // Move to end
            c.GotoPrev(MoveType.After, i => i.MatchLdloc(baseColorIndex));

            c.Emit(OpCodes.Pop); // Ditch the computed baseColor.
            c.Emit(OpCodes.Ldarg, 2); // Rare

            c.EmitDelegate<Func<int, Color>>(rarity =>
            {
                float colorPercentage = (float)Main.mouseTextColor / 255f;

                Color color = ItemRarity.GetColor(rarity);
                color = new Color(color.R * colorPercentage, color.G * colorPercentage, color.B * colorPercentage, (int)Main.mouseTextColor);

                return color;
            });
        }

        private void Main_OnMouseText_DrawItemTooltip(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            MethodInfo blackPropertyGetter = typeof(Color).GetProperty(nameof(Color.Black)).GetMethod;

            while (c.TryGotoNext(i => i.MatchCall(blackPropertyGetter)))
            {
            }

            int baseColorIndex = 0;
            c.GotoNext(i => i.MatchLdloca(out baseColorIndex));
            c.GotoNext(i => i.MatchLdcI4(-12));
            c.GotoNext(MoveType.After, i => i.MatchBr(out ILLabel endOfIf));

            c.Emit(OpCodes.Ldarg, 1);
            c.Emit(OpCodes.Ldloc, 64);

            c.EmitDelegate<Func<int, Color, Color>>((rarity, baseColor) =>
            {
                float colorPercentage = (float)Main.mouseTextColor / 255f;

                baseColor = ItemRarity.GetColor(rarity);
                baseColor = new Color(baseColor.R * colorPercentage, baseColor.G * colorPercentage, baseColor.B * colorPercentage, (float)Main.mouseTextColor);

                return baseColor;
            });

            string sil = c.Context.ToString();
        }


        /*
        private void Main_OnMouseText(On.Terraria.Main.orig_MouseText orig, Main self, string cursorText, int rare, byte diff, int hackedMouseX, int hackedMouseY, int hackedScreenWidth, int hackedScreenHeight)
        {
            if (Main.instance.mouseNPC > -1 || cursorText == null)
                return;


            int offsetX = Main.mouseX + 10;
            int offsetY = Main.mouseY + 10;


            if (hackedMouseX != -1 && hackedMouseY != -1)
            {
                offsetX = hackedMouseX + 10;
                offsetY = hackedMouseY + 10;
            }

            if (Main.ThickMouse)
            {
                offsetX += 6;
                offsetY += 6;
            }


            Vector2 vector = Main.fontMouseText.MeasureString(cursorText);

            if (Main.HoverItem.type > 0)
            {
                _drawItemTooltip.Invoke(Main.instance, new object[] { rare, diff, offsetX, offsetY });
                return;
            }

            if (Main.buffString != "" && Main.buffString != null)
            {
                _drawBuffString.Invoke(Main.instance, new object[] { offsetX, offsetY, (int)vector.Y });
            }


            if (hackedScreenHeight != -1 && hackedScreenWidth != -1)
            {
                if ((float)offsetX + vector.X + 4f > (float)hackedScreenWidth)
                {
                    offsetX = (int)((float)hackedScreenWidth - vector.X - 4f);
                }
                if ((float)offsetY + vector.Y + 4f > (float)hackedScreenHeight)
                {
                    offsetY = (int)((float)hackedScreenHeight - vector.Y - 4f);
                }
            }
            else
            {
                if ((float)offsetX + vector.X + 4f > (float)Main.screenWidth)
                {
                    offsetX = (int)((float)Main.screenWidth - vector.X - 4f);
                }
                if ((float)offsetY + vector.Y + 4f > (float)Main.screenHeight)
                {
                    offsetY = (int)((float)Main.screenHeight - vector.Y - 4f);
                }
            }

            float alpha = (float)Main.mouseTextColor / 255f;
            Color baseColor = ItemRarity.GetColor(rare);

            baseColor = new Color(baseColor.R * alpha, baseColor.G * alpha, baseColor.B * alpha, Main.mouseTextColor);

            if (diff == 1)
            {
                baseColor = new Color((int)((byte)((float)Main.mcColor.R * alpha)), (int)((byte)((float)Main.mcColor.G * alpha)), (int)((byte)((float)Main.mcColor.B * alpha)), (int)Main.mouseTextColor);
            }
            if (diff == 2)
            {
                baseColor = new Color((int)((byte)((float)Main.hcColor.R * alpha)), (int)((byte)((float)Main.hcColor.G * alpha)), (int)((byte)((float)Main.hcColor.B * alpha)), (int)Main.mouseTextColor);
            }


            ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, cursorText, new Vector2((float)offsetX, (float)offsetY), baseColor, 0f, Vector2.Zero, Vector2.One, -1f, 2f);
        }

        private void Main_OnMouseText_DrawItemTooltip(On.Terraria.Main.orig_MouseText_DrawItemTooltip orig, Main self, int rare, byte diff, int x, int y)
        {

        }
        */


        public static ModRarity GetRarity<T>() where T : ModRarity => Instance.GetGeneric<T>();

        public static int RarityType<T>() where T : ModRarity => Instance.GetGeneric<T>().Id;


        public bool HookingSuccessful { get; private set; }
    }
}