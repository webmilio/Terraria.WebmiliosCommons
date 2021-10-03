using System.Reflection;
using WebmilioCommons.Saving;

namespace WebmilioCommons.Reflection
{
    public class ReflectedSaveProperties
    {
        public ReflectedSaveProperties(PropertyInfo property)
        {
            Property = property;
        }


        public PropertyInfo Property { get; }

        public SaveAttribute Save { get; set; }
    }
}