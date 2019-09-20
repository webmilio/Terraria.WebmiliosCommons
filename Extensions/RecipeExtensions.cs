using Terraria.ModLoader;

namespace WebmilioCommons.Extensions
{
    public static class RecipeExtensions
    {
        public static void AddIngredient<T>(this ModRecipe recipe, int stack = 1) where T : ModItem
        {
            recipe.AddIngredient(typeof(T).GetModFromType().ItemType<T>(), stack);
        }
    }
}
