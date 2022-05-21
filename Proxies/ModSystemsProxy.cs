using System.Collections.Generic;
using System.Reflection;
using Terraria.ModLoader;

namespace WebmilioCommons.Proxies;

public class ModSystemsProxy : Proxy<ModSystem>
{
    protected override IList<ModSystem> GetSource()
    {
        return (IList<ModSystem>) typeof(SystemLoader).GetField("Systems", NormalFieldFlags).GetValue(null);
    }
}