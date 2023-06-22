using System.Linq;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

[assembly: ModInfo("Four Planks Log")]

namespace FourPlanksLog;

class Core : ModSystem
{
    public override bool ShouldLoad(EnumAppSide forSide) => forSide == EnumAppSide.Server;

    public override void StartServerSide(ICoreServerAPI api)
    {
        base.StartServerSide(api);

        foreach (GridRecipe recipe in api.World.GridRecipes)
        {
            recipe.ResolveIngredients(api.World);

            if (recipe.Output.ResolvedItemstack == null) continue;

            if (!recipe.Ingredients.Values.Any(IsLog)) continue;

            if (!IsPlank(recipe)) continue;

            recipe.Output.ResolvedItemstack.StackSize = 4;
        }

        api.World.Logger.Event("started 'Four Planks Log' mod");
    }

    private static bool IsLog(CraftingRecipeIngredient ingredient)
    {
        return ingredient?.Code.ToString().StartsWith("game:log") == true;
    }

    private static bool IsPlank(GridRecipe recipe)
    {
        return recipe.Output.Code.ToString().StartsWith("game:plank-");
    }
}