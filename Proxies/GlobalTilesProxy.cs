using System.Collections.Generic;
using Terraria.ModLoader;

namespace WebmilioCommons.Proxies;

public class GlobalTilesProxy : Proxy<GlobalTile, Tiles.GlobalTile>
{
    protected override IList<GlobalTile> GetSource()
    {
        return (IList<GlobalTile>) typeof(TileLoader).GetField("globalTiles", NormalFieldFlags).GetValue(null);
    }
}