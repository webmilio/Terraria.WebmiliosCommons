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


        internal override bool DoPreReceive(BinaryReader reader, int fromWho) => (Player = Main.player[SupposedPlayerID = reader.ReadInt32()]) != default;


        protected override void PreAssignValues(ref int? fromWho, ref int? toWho)
        {
            if (!fromWho.HasValue)
                Player = Main.player[(fromWho = SupposedPlayerID = Main.myPlayer).Value];
        }

        protected override void PrePopulatePacket(ModPacket modPacket, ref int fromWho, ref int toWho)
        {
            if (Player == null)
                Player = Main.player[SupposedPlayerID = fromWho];

            modPacket.Write(SupposedPlayerID);
        }


        [NotNetworkField]
        public int SupposedPlayerID { get; private set; }

        [NotNetworkField]
        public Player Player
        {
            get => ContextEntity as Player;
            set => ContextEntity = value;
        }
    }
}