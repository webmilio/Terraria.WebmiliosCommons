using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria.ModLoader;
using WebmilioCommons.Networking.Attributes;
using WebmilioCommons.Players;

namespace WebmilioCommons.Networking.Packets
{
    [Obsolete]
    public class PlayerSynchronizationPacket : ModPlayerNetworkPacket<WCPlayer>
    {
        private static Dictionary<TypeInfo, Mod> _modPlayersToSynchronizing;
        private static Dictionary<Mod, List<PropertyInfo>> _propertiesToSend;

        private readonly List<PlayerSynchronizationPacket> _toSend;

        public PlayerSynchronizationPacket() : base(false)
        {
            
        }

        public PlayerSynchronizationPacket(ModPlayer modPlayer)
        {
            CreatedWith = modPlayer;

            foreach (PropertyInfo propertyInfo in _propertiesToSend[modPlayer.mod])
                AddReaderWriter(propertyInfo);
        }

        public PlayerSynchronizationPacket(WCPlayer wcPlayer)
        {
            _toSend = new List<PlayerSynchronizationPacket>(_modPlayersToSynchronizing.Count);

            foreach (KeyValuePair<TypeInfo, Mod> modPlayer in _modPlayersToSynchronizing)
                _toSend.Add(new PlayerSynchronizationPacket(wcPlayer.player.GetModPlayer(modPlayer.Value, modPlayer.Key.Name)));
        }


        protected override void PopulatePacket(ModPacket modPacket, int? fromWho, int? toWho)
        {
            if (CreatedWith == null)
            {
                base.PopulatePacket(modPacket, fromWho, toWho);
                return;
            }

            foreach (PropertyInfo propertyInfo in _propertiesToSend[CreatedWith.mod])
                packetWriters[propertyInfo](modPacket, propertyInfo.GetValue(this));
        }


        internal static void Construct()
        {
            _modPlayersToSynchronizing = new Dictionary<TypeInfo, Mod>();
            _propertiesToSend = new Dictionary<Mod, List<PropertyInfo>>();

            foreach (Mod mod in ModLoader.Mods)
            {
                if (mod.Code == null)
                    continue;

                _propertiesToSend.Add(mod, new List<PropertyInfo>());

                foreach (TypeInfo typeInfo in mod.Code.DefinedTypes.Where(t => !t.IsAbstract && t.IsSubclassOf(typeof(ModPlayer))))
                {
                    foreach (PropertyInfo propertyInfo in typeInfo.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                    {
                        SynchronizeOnJoin attr = propertyInfo.GetCustomAttribute<SynchronizeOnJoin>();

                        if (attr == null)
                            continue;

                        _propertiesToSend[mod].Add(propertyInfo);
                    }

                    _modPlayersToSynchronizing.Add(typeInfo, mod);
                }
            }
        }


        [NotNetworkField]
        public ModPlayer CreatedWith { get; }
    }
}