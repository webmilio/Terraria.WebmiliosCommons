using System.Collections.Generic;
using Terraria.ModLoader.IO;

namespace WebmilioCommons.Commons;

public interface IHaveTags
{
    public Dictionary<string, object> Tags { get; }
}