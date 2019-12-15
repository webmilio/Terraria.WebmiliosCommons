using Terraria;
using Terraria.ModLoader;

namespace WebmilioCommons.Items
{
    public sealed partial class WCGlobalItemInstanced : GlobalItem
    {
        public override bool InstancePerEntity { get; } = true;
    }
}