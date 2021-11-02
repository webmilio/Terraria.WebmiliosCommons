using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria.ModLoader;
using WebmilioCommons.Extensions;

namespace WebmilioCommons.Helpers;

public class PlayerHelpers : ModSystem
{
    private static IList<ModPlayer> _players;

    public override void Load()
    {
        try
        {
            _players = (IList<ModPlayer>)typeof(PlayerLoader).GetField("players", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);
        }
        catch
        {
            Mod.Logger.ErrorFormat("Error while fetching the internal players list. None of the additional player hooks will be triggered.");
            _players = Array.Empty<ModPlayer>();
        }
    }

    public override void Unload()
    {
        _players = null;
    }

    /// <summary>Not optimal for actions called often, see <see cref="ForModPlayers"/></summary>
    /// <typeparam name="T"></typeparam>
    /// <returns>All ModPlayer instances of the specified type.</returns>
    public static List<T> GetModPlayers<T>()
    {
        List<T> items = new(_players.Count);

        _players.Do(delegate (ModPlayer player)
        {
            if (player is T t)
                items.Add(t);
        });

        return items;
    }

    public static void ForModPlayers(Action<ModPlayer> action)
    {
        _players.Do(action);
    }

    public static void ForModPlayers<T>(Action<T> action)
    {
        _players.Do(delegate(ModPlayer player)
        {
            if (player is T t)
                action(t);
        });
    }
}