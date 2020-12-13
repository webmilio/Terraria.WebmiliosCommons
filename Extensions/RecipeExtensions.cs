using Terraria.ModLoader;

namespace WebmilioCommons.Extensions
{
    public static class RecipeExtensions
    {
        /// <summary>Adds an ingredient to this recipe with the given item type and stack size.</summary>
        /// <typeparam name="T">The type of item to add.</typeparam>
        /// <param name="recipe"></param>
        /// <param name="stack">How many of the specified item are required in the recipe.</param>
        public static void AddIngredient<T>(this ModRecipe recipe, int stack = 1) where T : ModItem => 
            recipe.AddIngredient(ModContent.ItemType<T>(), stack);

        public static void AddTile<T>(this ModRecipe recipe) where T : ModTile =>
            recipe.AddTile(ModContent.TileType<T>());
    }
}
