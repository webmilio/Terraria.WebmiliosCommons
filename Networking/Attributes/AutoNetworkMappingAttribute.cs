using System;

namespace WebmilioCommons.Networking.Attributes
{
    [Obsolete("WIP"), AttributeUsage(AttributeTargets.Class)]
    public class AutoNetworkMappingAttribute : Attribute
    {
        public AutoNetworkMappingAttribute() : this(AutoNetworkMappingBehavior.OptIn)
        {
        }

        public AutoNetworkMappingAttribute(AutoNetworkMappingBehavior behavior)
        {
            Behavior = behavior;
        }


        public AutoNetworkMappingBehavior Behavior { get; }
    }
}