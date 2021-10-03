using System;
using System.Reflection;
using Terraria.ModLoader.IO;

namespace WebmilioCommons.Saving
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property)]
    public class SaveAttribute : Attribute
    {
        private readonly MethodInfo _getMethod = typeof(TagCompound).GetMethod(nameof(TagCompound.Get));

        public void Serialize(object owner, TagCompound tag, MemberProxy member)
        {
            tag.Add(GetKey(member), member.Get(owner));
        }

        public void Deserialize(object owner, TagCompound tag, MemberProxy member)
        {
            var serializer = _getMethod.MakeGenericMethod(member.Type);
            var value = serializer.Invoke(tag, new object[] { GetKey(member) });

            member.Set(owner, value);
        }

        public string GetKey(MemberProxy member)
        {
            return Key ?? member.Member.Name;
        }

        /// <summary>
        /// Sets the key under which the associated member will be saved.
        /// Not setting a value will make the key default to the member's name.
        /// </summary>
        public string Key { get; set; }
    }
}