using Terraria.ModLoader.IO;

namespace WebCom.Saving;

public interface ITagSerializable<T> : Terraria.ModLoader.IO.TagSerializable
{
    static abstract T Load(TagCompound tag);
}
