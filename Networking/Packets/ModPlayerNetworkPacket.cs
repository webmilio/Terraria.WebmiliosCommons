using System;
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


        internal override bool DoPreReceive(BinaryReader reader, int fromWho)
        {
            bool result = base.DoPreReceive(reader, fromWho);

            ModPlayer = ModPlayerGetter(Player);
            return result;
        }

        protected override void PreAssignValues(ref int? fromWho, ref int? toWho)
        {
            base.PreAssignValues(ref fromWho, ref toWho);

            ModPlayer = ModPlayerGetter(Player);
        }


        [NotNetworkField]
        public virtual T ModPlayer { get; set; }


        [NotNetworkField]
        public virtual Func<Player, T> ModPlayerGetter { get; } = player => player.GetModPlayer<T>();
    }
}
