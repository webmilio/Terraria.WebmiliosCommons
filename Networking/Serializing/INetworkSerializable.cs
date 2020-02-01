﻿using System.IO;
using Terraria.ModLoader;
using WebmilioCommons.Networking.Packets;

namespace WebmilioCommons.Networking.Serializing
{
    public interface INetworkSerializable
    {
        void Send(NetworkPacket networkPacket, ModPacket modPacket);

        void Receive(NetworkPacket networkPacket, BinaryReader reader);
    }
}