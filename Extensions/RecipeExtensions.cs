using Terraria;
using Terraria.ModLoader;

namespace WebmilioCommons.Extensions
{
    public static class RecipeExtensions
    {
        public static void AddTile<T>(this Recipe recipe) where T : ModTile =>
            recipe.AddTile(ModContent.TileType<T>());
    }
}
