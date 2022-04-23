using ReLogic.Content;
using Terraria;

namespace WebmilioCommons.Extensions;

public static class AssetExtensions
{
    public static Asset<T> GetAndLoad<T>(this Asset<T>[] assets, int index) where T : class
    {
        var asset = assets[index];
        return _GetAndLoad(asset);
    }

    public static T GetAndLoad<T>(this Asset<T> asset) where T : class
    {
        return _GetAndLoad(asset).Value;
    }

    private static Asset<T> _GetAndLoad<T>(this Asset<T> asset) where T : class
    {
        if (!asset.IsLoaded && asset.State != AssetState.Loading)
            return Main.Assets.Request<T>(asset.Name, AssetRequestMode.ImmediateLoad);

        return asset;
    }
}