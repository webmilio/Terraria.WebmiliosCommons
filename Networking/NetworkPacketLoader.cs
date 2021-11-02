using System;
using System.IO;
using System.Reflection;
using Terraria.DataStructures;
using Terraria.ModLoader;
using WebmilioCommons.Extensions;
using WebmilioCommons.Loaders;
using WebmilioCommons.Networking.Packets;
using WebmilioCommons.Networking.Packets.TileEntities;

namespace WebmilioCommons.Networking
{
    public sealed class NetworkPacketLoader : SingletonPrototypeLoader<NetworkPacketLoader, NetworkPacket>
    {
        public delegate void PacketReceivedDelegate(NetworkPacket packet, BinaryReader reader);
        public delegate void PacketSentDelegate(NetworkPacket packet);

        public NetworkPacketLoader() : base(typeInfo => !typeInfo.TryGetCustomAttribute(out ObsoleteAttribute _))
        {
            nextIndex = 1;
        }

        public override void PreLoad()
        {
            NetworkPacket.Initialize();
        }

        public override void PostLoad()
        {
            int lastPacketIndex = nextIndex - 1;

            if (lastPacketIndex <= byte.MaxValue)
            {
                PacketIdWriter = (packet, modPacket, value) => packet.WriteByte(modPacket, (byte)(int)value);
                PacketIdReader = (packet, reader) => (int)(byte)packet.ReadByte(reader);
            }
            else if (lastPacketIndex <= short.MaxValue)
            {
                PacketIdWriter = (packet, modPacket, value) => packet.WriteShort(modPacket, (short)(int)value);
                PacketIdReader = (packet, reader) => (int)(short)packet.ReadShort(reader);
            }
            else
            {
                PacketIdWriter = (packet, modPacket, value) => packet.WriteInt(modPacket, value);
                PacketIdReader = NetworkPacketIOExtensions.ReadInt;
            }
        }

        /// <summary></summary>
        protected override void PostUnload()
        {
            NetworkPacket.Unload();
        }

        /// <summary>Main method to hook into: redirect to this in your Mod's HandlePacket.</summary>
        /// <param name="reader"></param>
        /// <param name="fromWho"></param>
        public void HandlePacket(BinaryReader reader, int fromWho)
        {
            int typeId = (int)PacketIdReader(null, reader);
            NetworkPacket packet = New(typeId);

            PacketReceived?.Invoke(packet, reader);

            packet.Receive(reader, fromWho);
        }

        /// <summary>Sends a tile entity packet, providing it a tile entity.</summary>
        /// <typeparam name="TPacket">The type of packet to send.</typeparam>
        /// <param name="tileEntity">The associated tile entity.</param>
        public void SendTileEntityPacket<TPacket>(TileEntity tileEntity) where TPacket : TileEntityNetworkPacket => SendTileEntityPacket(New<TPacket>(), tileEntity);

        /// <summary>Sends a tile entity packet, providing it a tile entity.</summary>
        /// <param name="packet">The packet to send.</param>
        /// <param name="tileEntity">The associated tile entity.</param>
        public void SendTileEntityPacket(TileEntityNetworkPacket packet, TileEntity tileEntity) => packet.Send(tileEntity);

        /// <summary>Sends the given packet. The packet must not be a child of <see cref="TileEntityNetworkPacket"/>.</summary>
        /// <param name="packet">The packet to send.</param>
        /// <param name="fromWho">From who is the packet. Leaving this to its default value will make the packet rely on <see cref="NetworkPacket.AssignInitialValues"/>.</param>
        /// <param name="toWho">To who should the packet be sent. Leaving this to its default value will make the packet rely on <see cref="NetworkPacket.AssignInitialValues"/>.</param>
        public void SendPacket(NetworkPacket packet, int? fromWho = null, int? toWho = null)
        {
            if (packet is TileEntityNetworkPacket)
                throw new Exception($"You should use the {nameof(SendTileEntityPacket)} method to send ${nameof(TileEntityNetworkPacket)}.");

            packet.Send(fromWho, toWho);
        }

        /// <summary>Sends a new instance of the given packet. The packet must not be a child of <see cref="TileEntityNetworkPacket"/>.</summary>
        /// <param name="id">The packet id.</param>
        /// <param name="fromWho">From who is the packet. Leaving this to its default value will make the packet rely on <see cref="NetworkPacket.AssignInitialValues"/>.</param>
        /// <param name="toWho">To who should the packet be sent. Leaving this to its default value will make the packet rely on <see cref="NetworkPacket.AssignInitialValues"/>.</param>
        public void SendPacket(int id, int? fromWho = null, int? toWho = null) => SendPacket(New(id), fromWho, toWho);

        /// <summary>Sends a new instance of the given packet type. The packet must not be a child of <see cref="TileEntityNetworkPacket"/>.</summary>
        /// <param name="type">The packet's type.</param>
        /// <param name="fromWho">From who is the packet. Leaving this to its default value will make the packet rely on <see cref="NetworkPacket.AssignInitialValues"/>.</param>
        /// <param name="toWho">To who should the packet be sent. Leaving this to its default value will make the packet rely on <see cref="NetworkPacket.AssignInitialValues"/>.</param>
        public void SendPacket(Type type, int? fromWho = null, int? toWho = null) => SendPacket(GetId(type), fromWho, toWho);

        /// <summary>Sends a new instance of the given packet type. The packet must not be a child of <see cref="TileEntityNetworkPacket"/>.</summary>
        /// <typeparam name="TPacket">The packet's type.</typeparam>
        /// <param name="fromWho">From who is the packet. Leaving this to its default value will make the packet rely on <see cref="NetworkPacket.AssignInitialValues"/>.</param>
        /// <param name="toWho">To who should the packet be sent. Leaving this to its default value will make the packet rely on <see cref="NetworkPacket.AssignInitialValues"/>.</param>
        public void SendPacket<TPacket>(int? fromWho = null, int? toWho = null) where TPacket : NetworkPacket => SendPacket(typeof(TPacket), fromWho, toWho);

        /// <summary>The reader which is used to determine the packet id. Dynamically scales with how many packets (from 0 to 255, 255 to 65536, etc...).</summary>
        public Func<NetworkPacket, BinaryReader, object> PacketIdReader { get; private set; }

        /// <summary>The reader which is used to determine the packet id. Dynamically scales with how many packets (from 0 to 255, 255 to 65536, etc...).</summary>
        public Action<NetworkPacket, ModPacket, object> PacketIdWriter { get; private set; }

        internal static void OnPacketSent(NetworkPacket packet) => PacketSent?.Invoke(packet);

        public static event PacketReceivedDelegate PacketReceived;
        public static event PacketSentDelegate PacketSent;
    }
}