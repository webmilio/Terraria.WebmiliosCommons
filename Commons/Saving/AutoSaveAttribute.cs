using System;

namespace WebmilioCommons.Commons.Saving
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class AutoSaveAttribute : Attribute
    {
        public AutoSaveAttribute()
        {
        }

        public AutoSaveAttribute(string nbtKey)
        {
            NBTKey = nbtKey;

        }

        public string NBTKey { get; set; }
    }
}