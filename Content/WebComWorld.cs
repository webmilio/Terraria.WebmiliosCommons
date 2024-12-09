using System;
using System.Collections.Generic;
using System.IO;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using WebCom.Annotations;
using WebCom.Saving;

namespace WebCom.Content;

public class WebComWorld : ModSystem
{
    public delegate void IdentifierDelegate(Guid id);
    public event IdentifierDelegate IdentifierUpdated;

    public override void LoadWorldData(TagCompound tag)
    {
        Saver.This.Load(this, tag);
    }

    public override void SaveWorldData(TagCompound tag)
    {
        if (Identifier == default)
        {
            Identifier = Guid.NewGuid();
        }

        Saver.This.Save(this, tag);
    }

    private Queue<IdentifierDelegate> _identifierQueue = [];

    private Guid _identifier;
    [Save] public Guid Identifier
    {
        get => _identifier;
        set
        {
            if (Equals(value, _identifier))
            {
                return;
            }

            _identifier = value;

            for (int i = 0; i < _identifierQueue?.Count; i++)
            {
                var item = _identifierQueue.Dequeue();
                item(Identifier);
            }

            _identifierQueue = null;
        }
    }
    
    /// <summary>
    ///     Wait for the world's identifier to be set if it isn't already. Executes the <see cref="callback"/> immediately if it's already set.
    /// </summary>
    /// <param name="callback"></param>
    public void WaitForIdentifier(IdentifierDelegate callback)
    {
        if (_identifierQueue == null)
        {
            callback(Identifier);
        }
        else
        {
            _identifierQueue.Enqueue(callback);
        }
    }

    internal class WorldSynchronizePacket : Packet
    {
        protected override void PostReceive(BinaryReader reader, int fromWho)
        {
            Mod.Logger.InfoFormat("Received world's UUID: {0}", Id);
        }

        public Guid Id
        {
            get => ModContent.GetInstance<WebComWorld>().Identifier;
            set => ModContent.GetInstance<WebComWorld>().Identifier = value;
        }
    }
}