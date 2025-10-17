using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.ModLoader;
using WebCom.Extensions;

namespace WebCom.Net.v2;

public class ModMessageLoader
{
    private readonly Dictionary<int, Type> _packets = [];

    public ModMessageLoader(Mod mod)
    {
        int id = -1;
        var types = mod.Code.GetTypes().Concrete<IMessage>();

        foreach (var type in types)
        {
            MapMessage(id, type);
        }
    }

    internal void MapMessage(int currentIdx, Type type)
    {
        ValidateMessageType(type);


    }

    internal static void ValidateMessageType(Type type)
    {
        var ctors = type.GetConstructors();
        var paramlessCtor = ctors.FirstOrDefault(ctor => ctor.GetParameters().Length == 0) ??
            throw new NotSupportedException($"Could not find parameterless constructor for type {type}.");
    }
}
