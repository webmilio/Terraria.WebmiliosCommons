using System.Collections.Generic;
using System.Reflection;
using Terraria.ModLoader;

namespace WebmilioCommons.Helpers;

public class GlobalItemsProxy : ProxyHelper<GlobalItem>
{
    protected override IList<GlobalItem> GetOriginal()
    {
        return (IList<GlobalItem>)typeof(ItemLoader).GetField("globalItems", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);
    }
}