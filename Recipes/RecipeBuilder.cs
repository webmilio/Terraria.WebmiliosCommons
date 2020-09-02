using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Exceptions;
using WebmilioCommons.Extensions;

namespace WebmilioCommons.Recipes
{
    public class RecipeBuilder
    {
        private ModRecipe _recipe;
        private bool _added;


        /// <summary>Creates a new instance with no ingredients and no result.</summary>
        /// <seealso cref="SetResult"/>
        /// <seealso cref="Requires(int,int)"/>
        /// <seealso cref="Finish"/>
        public RecipeBuilder()
        {
        }

        /// <summary>Creates a new instance with no ingredients with the given item and stack as a result.</summary>
        /// <param name="mod">The mod who owns the recipe.</param>
        /// <param name="item">The item type.</param>
        /// <param name="stack">The stack.</param>
        /// <seealso cref="Requires(int,int)"/>
        /// <seealso cref="Finish"/>
        public RecipeBuilder(Mod mod, int item, int stack = 1)
        {
            New(mod, item, stack);
        }

        /// <summary>Creates a new instance with no ingredients with the given item and stack as a result.</summary>
        /// <param name="mod">The mod who owns the recipe.</param>
        /// <param name="itemName">Name of the item.</param>
        /// <param name="stack">The stack.</param>
        /// <seealso cref="Requires(int,int)"/>
        /// <seealso cref="Finish"/>
        public RecipeBuilder(Mod mod, string itemName, int stack = 1)
        {
            New(mod, itemName, stack);
        }

        /// <summary>Creates a new instance with no ingredients with the given item and stack as a result.</summary>
        /// <param name="item"></param>
        /// <param name="stack"></param>
        /// <seealso cref="Requires(int,int)"/>
        /// <seealso cref="Finish"/>
        public RecipeBuilder(ModItem item, int stack = 1)
        {
            New(item, stack);
        }


        /// <summary>Creates a new mod recipe for the given item and stack. If the previous recipe wasn't added, it adds the recipe then creates a new one.</summary>
        /// <param name="item">The mod item.</param>
        /// <param name="stack">The stack.</param>
        /// <returns></returns>
        public RecipeBuilder New(ModItem item, int stack)
        {
            _recipe = new ModRecipe(item.mod);
            _recipe.SetResult(item, stack);

            return this;
        }


        /// <summary>Creates a new mod recipe for the given item type and stack.</summary>
        /// <param name="mod">The mod who owns the recipe.</param>
        /// <param name="itemName">Name of the item.</param>
        /// <param name="stack">The stack.</param>
        /// <returns></returns>
        public RecipeBuilder New(Mod mod, string itemName, int stack)
        {
            _recipe = new ModRecipe(mod);
            _recipe.SetResult(mod, itemName, stack);

            return this;
        }

        /// <summary>Creates a new mod recipe for the given item type and stack. If the previous recipe wasn't added, it adds the recipe then creates a new one.</summary>
        /// <param name="mod">The mod who owns the recipe.</param>
        /// <param name="item">The item type.</param>
        /// <param name="stack">The stack.</param>
        /// <returns></returns>
        public RecipeBuilder New(Mod mod, int item, int stack)
        {
            _recipe = new ModRecipe(mod);
            _recipe.SetResult(item, stack);

            return this;
        }


        /// <summary>Clears the current recipe by calling <see cref="New(Terraria.ModLoader.ModItem,int)"/> and passing the existing recipe result.</summary>
        /// <returns></returns>
        public RecipeBuilder Clear() => New(_recipe.mod, _recipe.createItem.type, _recipe.createItem.stack);


        /// <summary>
        /// Adds an ingredient to this recipe with the given item type and stack size.
        /// Ex.: 
        /// <example>recipe.AddIngredient(ItemID.IronAxe)</example>
        /// </summary>
        /// <param name="type">The item type.</param>
        /// <param name="stack">The stack.</param>
        /// <returns></returns>
        public RecipeBuilder Requires(int type, int stack = 1)
        {
            _recipe.AddIngredient(type, stack);

            return this;
        }


        public RecipeBuilder Requires(params (short itemId, int stack)[] ingredients)
        {
            for (int i = 0; i < ingredients.Length; i++)
                Requires(ingredients[i].Item1, ingredients[i].Item2);

            return this;
        }


        /// <summary>
        /// Adds the specified ingredients to this recipe with the given item types.
        /// Ex.: 
        /// <example>recipe.AddIngredient(ItemID.IronAxe)</example>
        /// </summary>
        /// <param name="type1">The first item type.</param>
        /// <param name="type2">The second item type.</param>
        /// <param name="type3">The third item type.</param>
        /// <param name="types">The remaining item types.</param>
        /// <returns></returns>
        public RecipeBuilder Requires(int type1, int type2, int type3, params int[] types)
        {
            Requires(type1);
            Requires(type2);
            Requires(type3);


            for (int i = 0; i < types.Length; i++)
                Requires(types[i]);


            return this;
        }


        /// <summary>Adds an ingredient to this recipe with the given item name from the given mod, and with the given stack stack. If the mod parameter is null, then it will automatically use an item from the mod creating this recipe.</summary>
        /// <param name="mod">The mod.</param>
        /// <param name="itemName">Name of the item.</param>
        /// <param name="stack">The stack.</param>
        /// <returns></returns>
        /// <exception cref="RecipeException">The item " + itemName + " does not exist in mod " + mod.Name + ". If you are trying to use a vanilla item, try removing the first argument.</exception>
        public RecipeBuilder Requires(Mod mod, string itemName, int stack = 1)
        {
            _recipe.AddIngredient(mod ?? _recipe.mod, itemName, stack);

            return this;
        }


        // TODO Remove this when PR into TML.
        public RecipeBuilder Requires<T>(int stack = 1) where T : ModItem
        {
            _recipe.AddIngredient<T>(stack);

            return this;
        }


        /// <summary>Adds an ingredient to this recipe of the given type of item and stack size.</summary>
        /// <param name="modItem">The item.</param>
        /// <param name="stack">The stack.</param>
        /// <returns></returns>
        public RecipeBuilder Requires(ModItem modItem, int stack = 1)
        {
            _recipe.AddIngredient(modItem, stack);

            return this;
        }


        /// <summary>Adds a recipe group ingredient to this recipe with the given RecipeGroup name and stack size. Vanilla recipe groups consist of "Wood", "IronBar", "PresurePlate", "Sand", and "Fragment".</summary>
        /// <param name="recipeGroup">The name.</param>
        /// <param name="stack">The stack.</param>
        /// <returns></returns>
        /// <exception cref="RecipeException"></exception>
        public RecipeBuilder Requires(string recipeGroup, int stack = 1)
        {
            _recipe.AddRecipeGroup(recipeGroup, stack);

            return this;
        }


        /// <summary>
        /// Adds one or many required crafting station(s) with the given tile type(s) to the recipe being built.
        /// Ex.:
        /// <example>At(TileID.WorkBenches, TileID.Anvils)</example>
        /// </summary>
        /// <param name="tileTypes"></param>
        /// <returns></returns>
        public RecipeBuilder At(params int[] tileTypes)
        {
            for (int i = 0; i < tileTypes.Length; i++)
                _recipe.AddTile(tileTypes[i]);

            return Finish();
        }

        public RecipeBuilder Anywhere() => Finish();


        public RecipeBuilder RequiresLava() => NeedVar(recipe => recipe.needLava = true);

        public RecipeBuilder RequiresHoney() => NeedVar(recipe => recipe.needHoney = true);

        public RecipeBuilder RequiresWater() => NeedVar(recipe => recipe.needWater = true);

        public RecipeBuilder RequiresSnowBiome() => NeedVar(recipe => recipe.needSnowBiome = true);


        private RecipeBuilder NeedVar(Action<ModRecipe> need)
        {
            need(_recipe);

            return this;
        }


        /// <summary>Adds this recipe to the game. Call this after you have finished setting the result, ingredients, etc.</summary>
        /// <returns></returns>
        /// <exception cref="RecipeException">A recipe without any result has been added.</exception>
        private RecipeBuilder Finish()
        {
            _recipe.AddRecipe();
            Clear();

            return this;
        }


        // Inspired by Itorius' 'Makes'.
        // See post https://discordapp.com/channels/103110554649894912/445276626352209920/678226652127428630
        /// <summary>
        /// Sets the result of this recipe with the given item type and stack size.
        /// If this <see cref="RecipeBuilder"/> instance was created using a constructor in which the result is specified, use <see cref="Finish"/> instead./>
        /// </summary>
        /// <param name="itemId">The item identifier.</param>
        /// <param name="stack">The stack.</param>
        /// <returns></returns>
        /// <seealso cref="Finish"/>
        public RecipeBuilder Produces(int itemId, int stack = 1)
        {
            _recipe.SetResult(itemId, stack);

            return Finish();
        }

        // Inspired by Itorius' 'Makes'.
        // See post https://discordapp.com/channels/103110554649894912/445276626352209920/678226652127428630
        /// <summary>
        /// Sets the result of this recipe to the given type of item and stack size. Useful in ModItem.AddRecipes.
        /// If this <see cref="RecipeBuilder"/> instance was created using a constructor in which the result is specified, use <see cref="Finish"/> instead./>
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="stack">The stack.</param>
        /// <returns></returns>
        /// <seealso cref="Finish"/>
        public RecipeBuilder Produces(ModItem item, int stack = 1)
        {
            _recipe.SetResult(item, stack);

            return Finish();
        }

        // Inspired by Itorius' 'Makes'.
        // See post https://discordapp.com/channels/103110554649894912/445276626352209920/678226652127428630
        /// <summary>
        /// Sets the result of this recipe with the given item name from the given mod, and with the given stack stack. If the mod parameter is null, then it will automatically use an item from the mod creating this recipe. Useful in ModItem.AddRecipes.
        /// If this <see cref="RecipeBuilder"/> instance was created using a constructor in which the result is specified, use <see cref="Finish"/> instead./>
        /// </summary>
        /// <param name="mod">The mod the item originates from.</param>
        /// <param name="itemName">Name of the item.</param>
        /// <param name="stack">The stack.</param>
        /// <returns></returns>
        /// <seealso cref="Finish"/>
        public RecipeBuilder Produces(Mod mod, string itemName, int stack = 1)
        {
            _recipe.SetResult(mod, itemName, stack);

            return Finish();
        }


        /// <summary>Changes the result of the current recipe with the given item type and stack size..</summary>
        /// <param name="type">The type.</param>
        /// <param name="stack">The stack.</param>
        /// <returns></returns>
        public RecipeBuilder SetResult(int type, int stack)
        {
            _recipe.SetResult(type, stack);

            return this;
        }

        /// <summary>Changes how many of the output item is produced from the recipe.</summary>
        /// <param name="stack"></param>
        /// <returns></returns>
        public RecipeBuilder SetResultStack(int stack)
        {
            _recipe.SetResult(_recipe.createItem.type, stack);

            return this;
        }
    }


    public static class RecipeExtensions
    {
        public static RecipeBuilder BuildRecipe(this ModItem item, int resultStack = 1) => new RecipeBuilder(item, resultStack);
    }
}