using System;

namespace WebmilioCommons;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class NameAttribute : Attribute
{
    public NameAttribute(string name)
    {
        Name = name;
    }

    public string Name { get; }
}