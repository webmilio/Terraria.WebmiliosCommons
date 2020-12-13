using Terraria.ModLoader;
using WebmilioCommons.Managers;

namespace WebmilioCommons.Hair
{
    public abstract class ModHair : IHasUnlocalizedName
    {
        protected ModHair()
        {
        }

        public Mod Mod { get; internal set; }

        public string UnlocalizedName { get; }
    }
}