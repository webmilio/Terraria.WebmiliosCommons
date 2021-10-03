using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using WebmilioCommons.Networking.Attributes;

namespace WebmilioCommons.Networking.Packets
{
    public abstract class NPCNetworkPacket : NetworkPacket
    {
        protected NPCNetworkPacket()
        {
        }

        protected NPCNetworkPacket(NPC npc)
        {
            NPC = npc;
        }


        internal override bool DoPreReceive(BinaryReader reader, int fromWho) => (NPC = Main.npc[reader.ReadInt32()]) != default;

        protected override void PrePopulatePacket(ModPacket modPacket, ref int fromWho, ref int toWho)
        {
            if (NPC == default)
                throw new ArgumentException("NPC was not set before populating the packet!");

            modPacket.Write(NPC.whoAmI);
        }


        [NotMapped]
        public NPC NPC
        {
            get => (NPC) ContextEntity;
            set => ContextEntity = value;
        }
    }
}