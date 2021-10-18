using System.IO;
using Terraria;
using Terraria.ModLoader;
using WebmilioCommons.Commons.Players;

namespace WebmilioCommons.Networking.Packets
{
    public abstract class PlayerNetworkPacket : NetworkPacket, IPlayerLinked
    {
        protected PlayerNetworkPacket() : this(true) { }

        protected PlayerNetworkPacket(bool autoGetProperties) : base(autoGetProperties)
        {
        }

        protected PlayerNetworkPacket(Player player)
        {
            Player = player;
        }


        internal override bool DoPreReceive(BinaryReader reader, int fromWho) => (Player = Main.player[SupposedPlayerID = reader.ReadInt32()]) != default;


        protected override void PreAssignValues(ref int? fromWho, ref int? toWho)
        {
            if (!fromWho.HasValue)
                fromWho = SupposedPlayerID = Main.myPlayer;
        }

        protected override void PrePopulatePacket(ModPacket modPacket, ref int fromWho, ref int toWho)
        {
            if (Player == null)
                Player = Main.player[SupposedPlayerID = fromWho];

            modPacket.Write(Player.whoAmI);
        }


        [NotMapped]
        public int SupposedPlayerID { get; private set; }

        [NotMapped]
        public Player Player
        {
            get => ContextEntity as Player;
            set => ContextEntity = value;
        }
    }
}