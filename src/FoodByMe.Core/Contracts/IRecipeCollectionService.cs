using System.Collections.Generic;
using System.Threading.Tasks;
using FoodByMe.Core.Contracts.Data;

namespace FoodByMe.Core.Contracts
{
    public interface IRecipeCollectionService
    {
        Task<List<Recipe>> SearchRecipesAsync(RecipeQuery query);

        Task<Recipe> FindRecipeAsync(int recipeId);

        Task SaveRecipeAsync(Recipe recipe);

        Task RemoveRecipeAsync(int recipeId);

        Task ToggleRecipeFavoriteTagAsync(int recipeId, bool isFavorite);

        IReferenceBookService ReferenceBook { get; }
    }
}
