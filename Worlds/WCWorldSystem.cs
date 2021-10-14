using System;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace WebmilioCommons.Worlds;

public class WCWorldSystem : ModSystem
{
    public override void LoadWorldData(TagCompound tag)
    {
        UniqueId = Guid.Parse(tag.GetString(nameof(UniqueId)));
    }

    public override void SaveWorldData(TagCompound tag)
    {
        tag.Add(nameof(UniqueId), UniqueId.ToString());
    }
    
    public Guid UniqueId { get; internal set; } = Guid.NewGuid();
}