using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;

namespace WebmilioCommons.Helpers;

public class GlobalItemsProxy : ProxyHelper<GlobalItem, Item>
{
    protected override IList<GlobalItem> GetSource()
    {
        return (IList<GlobalItem>)typeof(ItemLoader).GetField("globalItems", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);
    }
}