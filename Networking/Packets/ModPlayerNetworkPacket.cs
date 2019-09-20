using System.IO;
using Terraria;
using Terraria.ModLoader;
using WebmilioCommons.Networking.Attributes;

namespace WebmilioCommons.Networking.Packets
{
    public abstract class ModPlayerNetworkPacket<T> : PlayerNetworkPacket where T : ModPlayer
    {
        protected ModPlayerNetworkPacket() : base(true) { }

        protected ModPlayerNetworkPacket(bool autoGetProperties) : base(autoGetProperties)
        {
        }


        protected override bool PreReceive(BinaryReader reader, int fromWho)
        {
            bool result = base.PreReceive(reader, fromWho);

            ModPlayer = Player.GetModPlayer<T>();
            return result;
        }

        protected override void PreAssignValues(ref int? fromWho, ref int? toWho)
        {
            base.PreAssignValues(ref fromWho, ref toWho);

            ModPlayer = Player.GetModPlayer<T>();
        }


        [NotNetworkField]
        public T ModPlayer { get; set; }
    }
}