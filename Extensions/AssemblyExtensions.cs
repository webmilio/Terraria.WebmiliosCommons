using System.Reflection;
using Terraria.ModLoader;

namespace WebmilioCommons.Extensions
{
    public static class AssemblyExtensions
    {
        public static Mod GetModFromAssembly(this Assembly assembly) => ModLoader.GetMod(assembly.GetName().Name);
    }
}
