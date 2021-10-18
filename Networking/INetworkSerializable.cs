using System;

namespace WebmilioCommons.Networking
{
    [Obsolete("Use INetworkSerializable in WebmilioCommons.Networking.Serializing", true)]
    public interface INetworkSerializable : Serializing.INetworkSerializable
    {
    }
}