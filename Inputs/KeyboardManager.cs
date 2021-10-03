using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.ModLoader;

namespace WebmilioCommons.Inputs
{
    public class KeyboardManager : ModSystem
    {
        private static bool _loaded;

        public delegate void KeyStateChanged(Keys key);

        private static Keys[] _allKeys;
        private static Dictionary<Keys, KeyStates> _keyStates;

        public static readonly List<Keys>
            notPressed = new List<Keys>(),
            pressed = new List<Keys>(),
            justPressed = new List<Keys>(),
            justReleased = new List<Keys>();


        internal static void Load()
        {
            _loaded = false;

            _allKeys = (Keys[])Enum.GetValues(typeof(Keys));

            _keyStates = new Dictionary<Keys, KeyStates>();

            for (int i = 0; i < _allKeys.Length; i++)
                _keyStates.Add(_allKeys[i], KeyStates.NotPressed);

            _loaded = true;
        }

        public override void Unload()
        {
            _loaded = false;
            
            _allKeys = null;

            _keyStates?.Clear();
            _keyStates = null;
        }

        public override void PostUpdateInput()
        {
            if (!_loaded)
                return;

            foreach (KeyValuePair<Keys, KeyStates> kvp in new Dictionary<Keys, KeyStates>(_keyStates))
            {
                Keys key = kvp.Key;

                KeyState keyState = Main.keyState[key];

                if (keyState == KeyState.Down)
                {
                    if (kvp.Value == KeyStates.JustPressed)
                        SetState(key, KeyStates.Pressed);
                    else if (kvp.Value == KeyStates.NotPressed)
                        SetState(key, KeyStates.JustPressed);
                }
                else if (keyState == KeyState.Up)
                {
                    if (kvp.Value == KeyStates.JustReleased)
                        SetState(key, KeyStates.NotPressed);
                    else if (kvp.Value == KeyStates.Pressed)
                        SetState(key, KeyStates.JustReleased);
                }
            }
        }


        private static void SetState(Keys key, KeyStates keyState)
        {
            _keyStates[key] = keyState;

            if (keyState.HasFlag(KeyStates.JustPressed))
            {
                notPressed.RemoveAll(k => k == key);
                justPressed.Add(key);
                pressed.Add(key);

                KeyPressed?.Invoke(key);
            }

            if (keyState.HasFlag(KeyStates.Pressed) && !keyState.HasFlag(KeyStates.JustPressed))
            {
                justPressed.RemoveAll(k => k == key);
            }

            if (keyState.HasFlag(KeyStates.JustReleased))
            {
                pressed.RemoveAll(k => k == key);
                notPressed.Add(key);
                justReleased.Add(key);

                KeyReleased?.Invoke(key);
            }

            if (keyState.HasFlag(KeyStates.NotPressed) && !keyState.HasFlag(KeyStates.JustReleased))
            {
                justReleased.RemoveAll(k => k == key);
            }
        }


        public static KeyStates GetKeyState(Keys key, bool ignoreBlockInput = false) => 
            (!Main.blockInput || ignoreBlockInput) ? _keyStates[key] : KeyStates.NotPressed;


        public static bool IsNotPressed(Keys key, bool ignoreBlockInput = false) =>Is(key, KeyStates.NotPressed, ignoreBlockInput);
        public static bool IsPressed(Keys key, bool ignoreBlockInput = false) => Is(key, KeyStates.Pressed, ignoreBlockInput);
        public static bool IsJustPressed(Keys key, bool ignoreBlockInput = false) => Is(key, KeyStates.JustPressed, ignoreBlockInput);
        public static bool IsJustReleased(Keys key, bool ignoreBlockInput = false) => Is(key, KeyStates.JustReleased, ignoreBlockInput);


        public static bool Is(Keys key, KeyStates keyState, bool ignoreBlockInput = false) =>
            (!Main.blockInput || ignoreBlockInput) && _keyStates[key].HasFlag(keyState);


        public static event KeyStateChanged KeyPressed, KeyReleased;
    }
}