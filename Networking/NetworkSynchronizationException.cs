using System;

namespace WebmilioCommons.Networking
{
    public class NetworkSynchronizationException : Exception
    {
        public NetworkSynchronizationException(string message, Exception innerException = null) : base(message, innerException)
        {
        }
    }
}