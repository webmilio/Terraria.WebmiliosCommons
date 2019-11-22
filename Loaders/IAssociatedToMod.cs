using Terraria.ModLoader;

namespace WebmilioCommons.Loaders
{
    /// <summary>Implementing this interface will cause the loader to automatically feed the owner <see cref="Terraria.ModLoader.Mod"/> to the requested instance when using <see cref="Loader{T}.New(ushort)"/> or <see cref="Loader{T}.New{TType}"/></summary>
    public interface IAssociatedToMod
    {
        Mod Mod { get; set; }
    }
}