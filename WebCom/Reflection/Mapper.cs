using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WebCom.Annotations;
using WebCom.Extensions;

namespace WebCom.Reflection;

public class Mapper
{
    private readonly Dictionary<Type, Dictionary<Type, LocalMapper>> _mappers = new();

    public T Map<T>(object src) => Map(src, Activator.CreateInstance<T>());

    public T Map<T>(object src, T dst)
    {
        _mappers.TryGetOrDefault(src.GetType(), out var mappings, t => new());
        mappings.TryGetOrDefault(typeof(T), out var mapper, (dstType) => MakeMapping(src.GetType(), dstType));

        return mapper.Map(src, dst);
    }

    private static LocalMapper MakeMapping(Type src, Type dst)
    {
        return new LocalMapper(src, dst);
    }

    private class LocalMapper
    {
        private readonly List<Tuple<MemberInfoWrapper, MemberInfoWrapper>> _properties = new();

        public LocalMapper(Type src, Type dst) // We COULD support bidirectional mapping but I don't think it's necessary for now.
        {
            const BindingFlags Flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase | 
                BindingFlags.SetProperty | BindingFlags.GetProperty | BindingFlags.SetField | BindingFlags.GetField;

            const StringComparison StringComparison = StringComparison.OrdinalIgnoreCase;

            var dstMembers = dst.GetDataMembers(Flags);

            foreach (var srcMember in src.GetDataMembers(Flags))
            {
                bool Comparison(MemberInfoWrapper d)
                {
                    return NameEquals(srcMember, d) && TypeEquals(srcMember, d);
                }

                bool NameEquals(MemberInfoWrapper s, MemberInfoWrapper d)
                {
                    return d.Member.Name.Equals(srcMember.Member.Name, StringComparison) ||
                        d.Member.TryGetCustomAttribute<NameAttribute>(out var nameAttr) &&
                        nameAttr.Name.Equals(srcMember.Member.Name, StringComparison);
                }

                bool TypeEquals(MemberInfoWrapper s, MemberInfoWrapper d)
                {
                    return d.Type == s.Type || d.Type.IsSubclassOf(s.Type);
                }

                var dstMember = dstMembers.FirstOrDefault(Comparison);

                if (dstMember != null)
                {
                    _properties.Add(new(srcMember, dstMember));
                }
            }
        }

        public T Map<T>(object inst) => Map(inst, Activator.CreateInstance<T>());

        public T Map<T>(object src, T dst)
        {
            for (int i = 0; i < _properties.Count; i++)
            {
                _properties[i].Item2.SetValue(dst, _properties[i].Item1.GetValue(src));
            }

            return dst;
        }
    }
}
