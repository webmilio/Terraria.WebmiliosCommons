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
    private static WireColor[] _colors = Enum.GetValues<WireColor>();

    private static Action<DoubleStack<Point16>, int> _hitWireDelegate;
    private static Action<int, int> _hitWireSingleDelegate;

    // internal static Dictionary<WireColor, MethodInfo> wireMethods;

    public static void ForAllColors(int i, int j, Action<WireColor> action)
    {
        for (var k = 0; k < _colors.Length; k++)
        {
            if (HasWire(i, j, _colors[k]))
            {
                action(_colors[k]);
            }
        }
    }
    
    public static void ForAllWires(int i, int j, Action<Tile, WireColor> action)
    {
        var tile = Main.tile[i, j];

        for (int k = 0; k < _colors.Length; k++)
        {
            action(tile, _colors[k]);
        }
    }

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

    public static bool HasWire(int i, int j, WireColor color)
    {
        var tile = Main.tile[i, j];

        return color switch
        {
            WireColor.Red => tile.RedWire,
            WireColor.Green => tile.GreenWire,
            WireColor.Blue => tile.BlueWire,
            WireColor.Yellow => tile.YellowWire,
            _ => false
        };
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