using System;
using System.IO;
using Terraria.ModLoader;
using WebmilioCommons.Networking.Packets;

namespace WebmilioCommons.Networking
{
    [Obsolete("Use INetworkSerializable in WebmilioCommons.Networking.Serializing", true)]
    public interface INetworkSerializable : Serializing.INetworkSerializable
    {
    }
}