using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace WebmilioCommons.Extensions
{
    public static class AssemblyExtensions
    {
        public static Mod GetModFromAssembly(this Assembly assembly) => ModLoader.GetMod(assembly.GetName().Name);
    }
}
