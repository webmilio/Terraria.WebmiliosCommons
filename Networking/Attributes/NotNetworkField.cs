using System;

namespace WebmilioCommons.Networking.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public class NotNetworkField : Attribute
    {
        public NotNetworkField()
        {
        }
    }
}