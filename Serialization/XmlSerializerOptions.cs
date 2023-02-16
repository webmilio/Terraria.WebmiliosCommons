using System;
using System.Collections.Generic;

namespace WebmilioCommons.Serialization;

public class XmlSerializerOptions
{
    internal static readonly List<XmlConverter> DefaultConverters = new()
    {
        new GenericXmlConverter<byte>(byte.Parse),
        new GenericXmlConverter<sbyte>(sbyte.Parse),
        new GenericXmlConverter<ushort>(ushort.Parse),
        new GenericXmlConverter<short>(short.Parse),
        new GenericXmlConverter<uint>(uint.Parse),
        new GenericXmlConverter<int>(int.Parse),
        new GenericXmlConverter<ulong>(ulong.Parse),
        new GenericXmlConverter<long>(long.Parse),
        new GenericXmlConverter<float>(float.Parse),
        new GenericXmlConverter<double>(double.Parse),
        new GenericXmlConverter<decimal>(decimal.Parse),
        new GenericXmlConverter<bool>(bool.Parse),
    };

    public IList<XmlConverter> Converters { get; }

    internal object Convert(Type type, string str)
    {
        if (Converters != null)
        {
            for (int i = 0; i < Converters.Count; i++)
            {
                if (Converters[i].CanConvert(type))
                    return Converters[i].Convert(str);
            }
        }

        for (int i = 0; i < DefaultConverters.Count; i++)
        {
            if (DefaultConverters[i].CanConvert(type))
                return DefaultConverters[i].Convert(str);
        }

        return null;
    }

    public abstract class XmlConverter
    {
        public abstract bool CanConvert(Type type);

        public abstract object Convert(string str);

        public virtual Type Type { get; }
    }

    public class GenericXmlConverter<T> : XmlConverter
    {
        private readonly Func<string, T> _converter;

        public GenericXmlConverter(Func<string, T> converter) => _converter = converter;

        public override bool CanConvert(Type type) => type.IsAssignableFrom(Type);
        public override object Convert(string str) => _converter(str);

        public override Type Type => typeof(T);
    }
}