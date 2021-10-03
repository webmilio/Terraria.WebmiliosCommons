using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using MonoMod.RuntimeDetour;
using WebmilioCommons.Extensions;

namespace WebmilioCommons.Networking.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public class NetworkFieldAttribute : Attribute
    {
        
    }
}