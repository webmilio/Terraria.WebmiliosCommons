using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using WebmilioCommons.Commons.Wiring;

namespace WebmilioCommons.Helpers;

public class WiringHelper : ModSystem
{
    private static Action<DoubleStack<Point16>, int> _hitWireDelegate;
    private static Action<int, int> _hitWireSingleDelegate;

    // internal static Dictionary<WireColor, MethodInfo> wireMethods;

    public static void HitWire(int i, int j, int wireType) => HitWire(new Point16(i, j), wireType);

    public static void HitWire(Point16 point, int wireType)
    {
        var stack = new DoubleStack<Point16>();
        stack.PushFront(point);

        HitWire(stack, wireType);
    }

    public static void HitWire(DoubleStack<Point16> next, int wireType)
    {
        _hitWireDelegate(next, wireType);
    }

    public static void HitWireSingle(int i, int j)
    {
        _hitWireSingleDelegate(i, j);
    }

    public override void Load()
    {
        var wiringType = typeof(Wiring);

        _hitWireDelegate = wiringType.GetMethod("HitWire", BindingFlags.NonPublic | BindingFlags.Static)
            .CreateDelegate<Action<DoubleStack<Point16>, int>>();

        _hitWireSingleDelegate = wiringType.GetMethod("HitWireSingle", BindingFlags.NonPublic | BindingFlags.Static)
            .CreateDelegate<Action<int, int>>();

        /*var colors = Enum.GetValues<WireColor>();
        wireMethods = new(colors.Length);

        var tileType = typeof(Tile);

        for (int i = 0; i < colors.Length; i++)
        {
            var name = "wire";

            if (i > 0)
                name += i + 1;

            var method = tileType.GetMethod(name, BindingFlags.NonPublic | BindingFlags.Instance, Array.Empty<Type>());
            wireMethods.Add(colors[i], method);
        }*/
    }

    public override void Unload()
    {
        // wireMethods = null;
    }
}

/*public static class WiringExtensions
{
    public static bool Wire(this Tile tile, WireColor color)
    {
        return (bool) WiringHelper.wireMethods[color].Invoke(tile, Array.Empty<object>());
    }
}*/