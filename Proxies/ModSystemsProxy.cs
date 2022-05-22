using System.Collections.Generic;
using Terraria.ModLoader;
using MS = Terraria.ModLoader.ModSystem;

namespace WebmilioCommons.Proxies;

public class ModSystemsProxy : Proxy<MS>
{
    protected override IList<MS> GetSource()
    {
        return (IList<MS>) typeof(SystemLoader).GetField("Systems", NormalFieldFlags).GetValue(null);
    }
}