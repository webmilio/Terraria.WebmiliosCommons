using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;
using WebmilioCommons.Extensions;
using WebmilioCommons.Tinq;

namespace WebmilioCommons.Proxies.Players
{
    public static class PlayerHooksProxy
    {
        private static readonly FieldInfo _modPlayersField;
        private static Dictionary<Player, ModPlayer[]> _modPlayers;


        static PlayerHooksProxy()
        {
            _modPlayersField = typeof(Player).GetField("modPlayers", BindingFlags.NonPublic | BindingFlags.Instance);
        }


        internal static void Load()
        {
            _modPlayers = new Dictionary<Player, ModPlayer[]>();
        }

        internal static void Unload()
        {
            _modPlayers = default;
        }


        internal static void RegisterPlayersModPlayer(Player player)
        {
            ModPlayer[] GetModPlayers()
            {
                return (ModPlayer[])_modPlayersField.GetValue(player);
            }


            if (_modPlayers.ContainsKey(player))
                _modPlayers[player] = GetModPlayers();
            else
                _modPlayers.Add(player, GetModPlayers());
        }

        internal static void UnRegisterPlayersModPlayer(Player player)
        {
            if (!_modPlayers.ContainsKey(player))
                return;

            _modPlayers.Remove(player);
        }

        internal static void ClearModPlayer() => _modPlayers.Clear();


        public static ModPlayer[] GetModPlayers(Player player)
        {
            if (!_modPlayers.ContainsKey(player))
                RegisterPlayersModPlayer(player);

            return _modPlayers[player];
        }


        // TODO For all below, check performance impact.
        public static void Do<T>(Action<T> action) where T : ModPlayer
        {
            foreach (var player in Main.player.Active())
            {
                var modPlayers = GetModPlayers(player);

                for (int i = 0; i < modPlayers.Length; i++)
                    if (modPlayers[i] is T t)
                        action(t);
            }
        }

        public static void Do<T>(Player player, Action<T> action) where T : ModPlayer
        {
            var modPlayers = GetModPlayers(player);

            for (int i = 0; i < modPlayers.Length; i++)
                if (modPlayers[i] is T t)
                    action(t);
        }


        public static bool All<T>(Predicate<T> predicate) where T : ModPlayer
        {
            foreach (var player in Main.player.Active())
            {
                var modPlayers = GetModPlayers(player);

                for (int i = 0; i < modPlayers.Length; i++)
                    if (modPlayers[i] is T t && !predicate(t))
                        return false;
            }

            return true;
        }

        public static bool All<T>(Player player, Predicate<T> predicate) where T : ModPlayer
        {
            var modPlayers = GetModPlayers(player);

            for (int i = 0; i < modPlayers.Length; i++)
                if (modPlayers[i] is T t && !predicate(t))
                    return false;

            return true;
        }
    }
}