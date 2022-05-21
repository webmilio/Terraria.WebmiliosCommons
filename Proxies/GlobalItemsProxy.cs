using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;
using WebmilioCommons.Helpers;

namespace WebmilioCommons.Proxies;

public class GlobalItemsProxy : Proxy<GlobalItem, Item>
{
    protected override IList<GlobalItem> GetSource()
    {
        return (IList<GlobalItem>)typeof(ItemLoader).GetField("globalItems", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);
    }
}