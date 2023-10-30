using System;
using System.IO;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using WebCom.Annotations;
using WebCom.Saving;

namespace WebCom.Content;

public class WebComWorld : ModSystem
{
    public override void LoadWorldData(TagCompound tag)
    {
        Saver.This.Load(this, tag);
    }

    public override void SaveWorldData(TagCompound tag)
    {
        Saver.This.Save(this, tag);
    }

    public Guid Identifier { get; private set; } = Guid.NewGuid();
    [Save]
    private string SIdentifier
    {
        get => Identifier.ToString();
        set => Identifier = Guid.Parse(value);
    }

    internal class WorldSynchronizePacket : Packet
    {
        protected override void PostReceive(BinaryReader reader, int fromWho)
        {
            Mod.Logger.InfoFormat("Received world's UUID: {0}", Identifier);
        }

        public string Identifier
        {
            get => ModContent.GetInstance<WebComWorld>().SIdentifier;
            set => ModContent.GetInstance<WebComWorld>().SIdentifier = value;
        }
    }
}