using System;
using System.Collections;
using Terraria.ModLoader.IO;
using WebmilioCommons.Extensions;

namespace WebmilioCommons.Saving
{
    internal class MembersProxySerializer<T> : ProxySerializer<T>
    {
        private readonly SaveMemberProxy[] _proxies;

        public MembersProxySerializer(SaveMemberProxy[] proxies)
        {
            _proxies = proxies;
        }

        public override TagCompound Serialize(T value)
        {
            TagCompound tag = new();
            _proxies.Do(proxy => proxy.Serialize(value, tag));

            return tag;
        }

        public override T Deserialize(TagCompound tag)
        {
            var instance = Activator.CreateInstance<T>();
            _proxies.Do(proxy => proxy.Deserialize(instance, tag));

            return instance;
        }
    }
}