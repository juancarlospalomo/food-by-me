using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodByMe.Core.Contracts
{
    public interface IDatabaseService
    {
        Task UpdateDatabaseToLatestVersionAsync();

        Task<Recipe> FindRecipeAsync(int id);

        Task<Recipe> SaveRecipeAsync(Recipe recipe);

        Task<List<Recipe>> SearchRecipesAsync(RecipeQuery query);

        Task<List<RecipeCategory>> ListCategoriesAsync();

        Task<List<Measure>> ListMeasuresAsync();
    }
}
