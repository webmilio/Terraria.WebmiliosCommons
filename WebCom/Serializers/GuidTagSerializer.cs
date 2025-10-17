using System;
using Terraria.ModLoader.IO;

namespace WebCom.Serializers;

public class GuidTagSerializer : TagSerializer<Guid, string>
{
    public override Guid Deserialize(string tag)
    {
        return Guid.Parse(tag);
    }

    public override string Serialize(Guid value)
    {
        return value.ToString();
    }
}
