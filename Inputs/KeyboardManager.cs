using System;
using System.Linq;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.ModLoader;
using WebmilioCommons.Extensions;

namespace WebmilioCommons.Inputs
{
    public class KeyboardManager : ModSystem
    {
        private readonly Keys[] _keys;

        public readonly bool[] pressed;
        public readonly bool[] justPressed;

        public readonly bool[] released;
        public readonly bool[] justReleased;

        public KeyboardManager()
        {
            _keys = Enum.GetValues<Keys>();
            var maxId = _keys.Max(k => (int)k);

            pressed = InitializeArray(maxId);
            justPressed = InitializeArray(maxId);

            released = InitializeArray(maxId);
            justReleased = InitializeArray(maxId);
        }

        private static bool[] InitializeArray(int max) => new bool[max + 1];

        public override void PostUpdateInput()
        {
            _keys.Do(UpdateKey);
        }

        private void UpdateKey(Keys key)
        {
            var state = Main.keyState[key];
            int iKey = (int)key;

            justPressed[iKey] = false; // Keys can only be pressed for one tick.
            justReleased[iKey] = false; // Keys can only be just-released for one tick.

            if (state == KeyState.Down) // The key is pressed.
            {
                if (!pressed[iKey]) // We didn't know it was pressed before right now.
                {
                    justPressed[iKey] = true;
                }

                pressed[iKey] = true; // We now know that the key is pressed.
                released[iKey] = false;
            }
            else // They key is not pressed.
            {
                if (!released[iKey])
                {
                    justReleased[iKey] = true;
                }

                released[iKey] = true; // We know that the key is released.
                pressed[iKey] = false;
            }
        }

        /*private bool _loaded;

        public delegate void KeyStateChanged(Keys key);

        private Keys[] _allKeys;
        private Dictionary<Keys, KeyStates> _keyStates;

        public readonly List<Keys>
            notPressed = new(),
            pressed = new(),
            justPressed = new(),
            justReleased = new();

        public override bool IsLoadingEnabled(Mod mod)
        {
            return Main.netMode != NetmodeID.Server;
        }

        public override void Load()
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


        private void SetState(Keys key, KeyStates keyState)
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


        public KeyStates GetKeyState(Keys key, bool ignoreBlockInput = false) => 
            (!Main.blockInput || ignoreBlockInput) ? _keyStates[key] : KeyStates.NotPressed;


        public bool IsNotPressed(Keys key, bool ignoreBlockInput = false) =>Is(key, KeyStates.NotPressed, ignoreBlockInput);
        public bool IsPressed(Keys key, bool ignoreBlockInput = false) => Is(key, KeyStates.Pressed, ignoreBlockInput);
        public bool IsJustPressed(Keys key, bool ignoreBlockInput = false) => Is(key, KeyStates.JustPressed, ignoreBlockInput);
        public bool IsJustReleased(Keys key, bool ignoreBlockInput = false) => Is(key, KeyStates.JustReleased, ignoreBlockInput);


        public bool Is(Keys key, KeyStates keyState, bool ignoreBlockInput = false) =>
            (!Main.blockInput || ignoreBlockInput) && _keyStates[key].HasFlag(keyState);


        public event KeyStateChanged KeyPressed, KeyReleased;*/
    }
}