using System;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace WebmilioCommons.ModCompatibilities
{
    public abstract class ModCompatibility
    {
        protected ModCompatibility(string modName)
        {
            ModName = modName;
        }


        internal ModCompatibility TryRegister()
        {
            ModInstance = ModLoader.GetMod(ModName);

            if (ModInstance == null || !TryRegister(ModInstance))
                return null;

            return this;
        }

        protected virtual bool TryRegister(Mod mod) => true;


        public virtual void Load() { }

        public virtual void PostSetupContent() { } 


        public void TryAddRecipes()
        {
            try
            {
                AddRecipes();
            }
            catch (Exception e)
            {
                CallerMod.Logger.Error($"Error while adding recipes from `{ModInstance.Name}` for mod `{CallerMod.Name}`.", e);
            }
        }

        protected virtual void AddRecipes() { }


        public void TryAddRecipeGroups()
        {
            try
            {
                AddRecipeGroups();
            }
            catch (Exception e)
            {
                CallerMod.Logger.Error($"Error while adding recipe groups from `{ModInstance.Name}` for mod `{CallerMod.Name}`.", e);
            }
        }

        protected virtual void AddRecipeGroups() { }


        public Mod CallerMod { get; internal set; }

        public string ModName { get; }
        public Mod ModInstance { get; private set; }
    }
}