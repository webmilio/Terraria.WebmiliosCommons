using System;

namespace WebCom.Annotations;

public class NameAttribute : Attribute
{
    public NameAttribute(string name)
    {
        Name = name;
    }

    public string Name { get; }
}
