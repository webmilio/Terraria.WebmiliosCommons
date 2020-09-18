using System.Reflection;
using Microsoft.Xna.Framework.Input;
using MonoMod.RuntimeDetour.HookGen;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace WebmilioCommons
{
    public partial class WebmilioCommonsMod
    {
        private delegate void Orig_AddMenuButtons(Main main, int selectedMenu, string[] buttonNames, float[] buttonScales, ref int offY, ref int spacing, ref int buttonIndex, ref int numButtons);

        private delegate void Hook_AddMenuButtons(Orig_AddMenuButtons orig, Main main, int selectedMenu, string[] buttonNames, float[] buttonScales, ref int offY, ref int spacing, ref int buttonIndex, ref int numButtons);


        private void LoadAchievementsMenuHookThingLMAO()
        {
            On_AddMenuButtons += Interface_AddMenuButtons;
        }

        private void UnloadAchievementsMenuHookThingLMAO()
        {
            On_AddMenuButtons -= Interface_AddMenuButtons;
        }


        private static void Interface_AddMenuButtons(Orig_AddMenuButtons orig, Main main, int selectedMenu, string[] buttonNames, float[] buttonScales, ref int offY, ref int spacing, ref int buttonIndex, ref int numButtons)
        {
            buttonNames[buttonIndex] = Language.GetTextValue("UI.Achievements");

            if (selectedMenu == buttonIndex)
            {
                Main.PlaySound(SoundID.MenuOpen);
                Main.MenuUI.SetState(Main.AchievementsMenu); //I forgot what the achievementmenu is actually called :shrug:
                Main.menuMode = 888;
            }

            buttonIndex++;
            numButtons++;
            orig(main, selectedMenu, buttonNames, buttonScales, ref offY, ref spacing, ref buttonIndex, ref numButtons);
        }


        private static event Hook_AddMenuButtons On_AddMenuButtons
        {
            add
            {
                var method = typeof(Mod).Assembly.GetType("Terraria.ModLoader.UI.Interface").GetMethod("AddMenuButtons", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);
                HookEndpointManager.Add<Hook_AddMenuButtons>(method, value);
            }
            remove
            {
                var method = typeof(Mod).Assembly.GetType("Terraria.ModLoader.UI.Interface").GetMethod("AddMenuButtons", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);
                HookEndpointManager.Remove<Hook_AddMenuButtons>(method, value);
            }
        }
    }
}
