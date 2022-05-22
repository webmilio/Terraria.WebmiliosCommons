using System.Collections.Generic;
using System.Reflection;
using Terraria.ModLoader;

namespace WebmilioCommons.Proxies;

public class GlobalItemsProxy : Proxy<GlobalItem>
{
    protected override IList<GlobalItem> GetSource()
    {
        return (IList<GlobalItem>)typeof(ItemLoader).GetField("globalItems", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);
    }
}