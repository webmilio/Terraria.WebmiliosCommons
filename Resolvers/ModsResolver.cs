using System.Collections.Generic;
using Terraria.ModLoader;
using WebCom.DependencyInjection;
using WebCom.Extensions;

namespace WebCom.Resolvers;

[Service]
public class ModsResolver : Resolver<Mod>
{
    private IEnumerable<Mod> _assemblies;

    public override IEnumerable<Mod> Resolve()
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