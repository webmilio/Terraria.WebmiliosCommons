using System.Reflection;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace WebCom.Proxies;

internal class TagSerializerProxy : ModSystem
{
    private delegate void AddSerializerDelegate(TagSerializer serializer);
    private AddSerializerDelegate _addSerializer;

    internal void AddSerializer(TagSerializer serializer) => _addSerializer(serializer);

    public override void Load()
    {
        _addSerializer = typeof(TagSerializer).GetMethod(nameof(AddSerializer), BindingFlags.Static | BindingFlags.NonPublic)
            .CreateDelegate<AddSerializerDelegate>();
    }
}