using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using WebmilioCommons.Commons.Players;
using WebmilioCommons.Networking.Attributes;

namespace WebmilioCommons.Networking.Packets
{
    public abstract class ModPlayerNetworkPacket<T> : PlayerNetworkPacket, IModPlayerLinked<T> where T : ModPlayer
    {
        protected ModPlayerNetworkPacket() : base(true) { }

        protected ModPlayerNetworkPacket(bool autoGetProperties) : base(autoGetProperties)
        {
        }

        protected ModPlayerNetworkPacket(T modPlayer) : base(modPlayer.player)
        {
            ModPlayer = modPlayer;
        }


        internal override bool DoPreReceive(BinaryReader reader, int fromWho)
        {
            bool result = base.DoPreReceive(reader, fromWho);

            if (ModPlayer == null)
                ModPlayer = ModPlayerGetter(Player);

            return result;
        }

        /*protected override void PreAssignValues(ref int? fromWho, ref int? toWho)
        {
            base.PreAssignValues(ref fromWho, ref toWho);

            ModPlayer = ModPlayerGetter(Player);
        }*/

        protected override void PrePopulatePacket(ModPacket modPacket, ref int fromWho, ref int toWho)
        {
            base.PrePopulatePacket(modPacket, ref fromWho, ref toWho);

            if (ModPlayer == null)
                ModPlayer = ModPlayerGetter(Player);
        }


        [NotNetworkField]
        public virtual T ModPlayer { get; set; }


        [NotNetworkField]
        public virtual Func<Player, T> ModPlayerGetter { get; } = player => player.GetModPlayer<T>();
    }
}
