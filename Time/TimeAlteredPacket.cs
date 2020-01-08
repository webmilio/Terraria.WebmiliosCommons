using System;
using System.IO;
using System.Reflection;
using Terraria.ModLoader;
using WebmilioCommons.Networking.Attributes;
using WebmilioCommons.Networking.Packets;

namespace WebmilioCommons.Time
{
    public sealed class TimeAlteredPacket : NetworkPacket
    {
        public TimeAlteredPacket()
        {
        }

        public TimeAlteredPacket(TimeAlterationRequest request)
        {
            Request = request;
        }


        protected override void PrePopulatePacket(ModPacket modPacket, ref int fromWho, ref int toWho)
        {
            modPacket.Write(Request.GetType().FullName);
        }

        protected override bool PreReceive(BinaryReader reader, int fromWho)
        {
            string requestType = reader.ReadString();
            Request = Activator.CreateInstance(ExecutingAssembly.GetType(requestType)) as TimeAlterationRequest;

            return base.PreReceive(reader, fromWho);
        }

        protected override bool PostReceive(BinaryReader reader, int fromWho)
        {
            TimeManagement.TryAlterTime(Request, false);

            return base.PostReceive(reader, fromWho);
        }


        public TimeAlterationRequest Request { get; set; }

        [NotNetworkField]
        internal static Assembly ExecutingAssembly { get; set; }
    }
}