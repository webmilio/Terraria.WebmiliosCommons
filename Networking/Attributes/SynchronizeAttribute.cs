using System;
using WebmilioCommons.Commons;

namespace WebmilioCommons.Networking.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SynchronizeAttribute : Attribute
    {
        public ushort Id { get; internal set; }

        public EntityType Entity { get; internal set; }
    }
}