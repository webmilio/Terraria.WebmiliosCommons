using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using WebmilioCommons.Time;

namespace WebmilioCommons.Worlds
{
    public sealed class WCWorld : ModWorld
    {
        public override void Initialize()
        {
            if (UniqueID == Guid.Empty)
                UniqueID = Guid.NewGuid();
        }


        public override void PostDrawTiles()
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                Update();
        }

        public override void PostWorldGen()
        {
            UniqueID = Guid.NewGuid();
        }


        public override void PreUpdate()
        {
            if (Main.netMode == NetmodeID.SinglePlayer || Main.netMode == NetmodeID.Server)
                Update();
        }


        private void Update()
        {
            TimeManagement.Update();
        }


        public override void Load(TagCompound tag)
        {
            UniqueID = Guid.Parse(tag.GetString(nameof(UniqueID)));
        }

        public override TagCompound Save() => new TagCompound()
        {
            {nameof(UniqueID), UniqueID.ToString()}
        };


        public Guid UniqueID { get; private set; }
    }
}
