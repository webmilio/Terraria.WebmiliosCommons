using System.Collections.Generic;
using Terraria.ModLoader;
using WebCom.DependencyInjection;
using WebCom.Extensions;

namespace WebCom.Resolvers;

public class ModsProvider : IProvider<Mod>
{
    private IEnumerable<Mod> _assemblies;

    public IEnumerable<Mod> Provide()
    {
        if (Cache)
        {
            _assemblies ??= ModLoader.Mods.GetNonNullAssemblyMods();
            return _assemblies;
        }
        else
        {
            return ModLoader.Mods.GetNonNullAssemblyMods();
        }
    }

    public bool Cache { get; set; } = true;
}