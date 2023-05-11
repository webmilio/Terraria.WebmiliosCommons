using System;
using System.Collections.Generic;
using Terraria.ModLoader.IO;
using WebCom.Extensions;
using WebCom.Saving.Serializations;

namespace WebCom.Saving;

public class Saver
{
    private readonly SaveSerializers _mapper = new SaveSerializers.DynamicSaveSerializers();
    private readonly Dictionary<Type, SaveMapper> _savers = new();

    public void Save(object obj, TagCompound data)
    {
        var saver = Get(obj.GetType());
        saver.Save(obj, data);
    }

    public void Load(object obj, TagCompound data)
    {
        var saver = Get(obj.GetType());
        saver.Load(obj, data);
    }

    private SaveMapper Get(Type type)
    {
        return _savers.GetOrDefault(type, Map);
    }

    private SaveMapper Map(Type type)
    {
        return new SaveMapper(type, _mapper);
    }

    public static Saver This { get; private set; }
}
