using System;
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
        public static NetworkPacket GetPacket(this Mod mod, ushort id) => NetworkPacketLoader.Instance.New(id);

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
        public static ushort PacketId(this Mod mod, NetworkPacket packet) => NetworkPacketLoader.Instance.GetId(packet);
        public static ushort PacketId<T>(this Mod mod) where T : NetworkPacket => NetworkPacketLoader.Instance.GetId<T>();
    }
}