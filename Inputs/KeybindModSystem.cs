using System.Reflection;
using Terraria.ModLoader;
using WebmilioCommons.Extensions;

namespace WebmilioCommons.Inputs
{
    public class KeybindModSystem : ModSystem
    {
        public override void Load()
        {
            ModStore.ForTypes<Mod>(delegate(Mod mod, TypeInfo type)
            {
                var proxies = type.GetFieldsAndProperties();

                for (int i = 0; i < proxies.Count; i++)
                {
                    if (!proxies[i].Member.TryGetCustomAttribute(out KeybindAttribute keybind))
                        continue;

                    keybind.RegisterKeybind(mod, proxies[i].Set);
                }
            });
        }
    }
}