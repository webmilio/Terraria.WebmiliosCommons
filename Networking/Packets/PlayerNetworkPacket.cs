using System.IO;
using Terraria;
using Terraria.ModLoader;
using WebmilioCommons.Networking.Attributes;

namespace WebmilioCommons.Networking.Packets
{
    public abstract class PlayerNetworkPacket : NetworkPacket
    {
        protected PlayerNetworkPacket() : this(true) { }

        protected PlayerNetworkPacket(bool autoGetProperties) : base(autoGetProperties)
        {
        }


        protected override bool PreReceive(BinaryReader reader, int fromWho)
        {
            int whichPlayer = reader.ReadInt32();
            Player = Main.player[whichPlayer];

            return true;
        }


        protected override void PreAssignValues(ref int? fromWho, ref int? toWho)
        {
            if (!fromWho.HasValue)
            {
                Player = Main.LocalPlayer;
                fromWho = Player.whoAmI;
            }
        }

        protected override void PrePopulatePacket(ModPacket modPacket, ref int fromWho, ref int toWho)
        {
            if (Player == null)
                Player = Main.player[fromWho];

            modPacket.Write(Player.whoAmI);
        }


        [NotNetworkField]
        public virtual Player Player
        {
            get => ContextEntity as Player;
            set => ContextEntity = value;
        }
    }
}