using System;
using System.Collections.Generic;
using Terraria.ModLoader;
using WebmilioCommons.Networking;
using WebmilioCommons.Networking.Packets;

namespace WebmilioCommons.Extensions
{
    public static class ModExtensions
    {
        /// <summary>Returns a new instance of the packet corresponding to the specified ID.</summary>
        /// <param name="mod"></param>
        /// <param name="id">The auto-generated ID of the packet.</param>
        /// <returns>A new <see cref="NetworkPacket"/>.</returns>
        public static NetworkPacket GetPacket(this Mod mod, int id) => NetworkPacketLoader.Instance.New(id);

        /// <summary>Returns a new instance of the packet corresponding to the specified type.</summary>
        /// <param name="mod"></param>
        /// <param name="type">The class type of the packet.</param>
        /// <returns>A new <see cref="NetworkPacket"/>.</returns>
        public static NetworkPacket GetPacket(this Mod mod, Type type) => NetworkPacketLoader.Instance.New(type);
        public static T GetPacket<T>(this Mod mod) where T : NetworkPacket => GetPacket(mod, typeof(T)) as T;

        /// <summary>Returns the auto-generated ID for the specified packet.</summary>
        /// <param name="mod"></param>
        /// <param name="packet">The packet.</param>
        /// <returns>The auto-generated ID of the packet.</returns>
        public static int PacketId(this Mod mod, NetworkPacket packet) => NetworkPacketLoader.Instance.GetId(packet);
        public static int PacketId<T>(this Mod mod) where T : NetworkPacket => NetworkPacketLoader.Instance.GetId<T>();


        /// <summary>Filters out mods that have a <c>null</c> <see cref="Mod.Code"/>.</summary>
        /// <param name="mods"></param>
        /// <returns>A list of filtered mods.</returns>
        public static List<Mod> StandardModFilter(this IList<Mod> mods)
        {
            List<Mod> filtered = new List<Mod>();

            for (int i = 0; i < mods.Count; i++)
                if (mods[i].Code != null)
                    filtered.Add(mods[i]);

            return filtered;
        }
    }
}