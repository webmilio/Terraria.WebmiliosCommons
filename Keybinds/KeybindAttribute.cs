using Microsoft.Xna.Framework.Input;
using System;
using Terraria.ModLoader;

namespace WebCom.Keybinds;

[AttributeUsage(AttributeTargets.Property)]
public class KeybindAttribute : Attribute
{
    public KeybindAttribute(string name, string defaultHotkey)
    {
        Name = name;
        DefaultHotkey = defaultHotkey;
    }

    public KeybindAttribute(string name, Keys defaultHotkey) : this(name, defaultHotkey.ToString()) { }

    public string Name { get; }
    public string DefaultHotkey { get; }
}