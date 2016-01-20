using System.Collections.Generic;
using System.Threading.Tasks;
using FoodByMe.Core.Contracts.Data;

namespace FoodByMe.Core.Contracts
{
    public interface ISearchService
    {
        Task<Recipe> FindRecipeAsync(int id);

        Task<List<Recipe>> SearchRecipesAsync(RecipeQuery query);

        Task<List<string>> SearchIngredients(string query);
    }
}
