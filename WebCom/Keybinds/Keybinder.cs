using System;
using Terraria.ModLoader;
using WebCom.Extensions;

namespace WebCom.Keybinds;

public class Keybinder
{
    internal static void Register(Mod mod, object obj)
    {
        var members = obj.GetType().GetDataMembers();

        foreach (var member in members)
        {
            if (member.Member.TryGetCustomAttribute(out KeybindAttribute kb))
            {
                member.SetValue(obj, KeybindLoader.RegisterKeybind(mod, kb.Name, kb.DefaultHotkey));
            }
        }
    }

    public static void Register(Mod mod)
    {
        Register(mod, mod);
    }

    public static void Register(IModType obj)
    {
        Register(obj.Mod, obj);
    }
}