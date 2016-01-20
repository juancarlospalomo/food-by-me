using System.Collections.Generic;
using FoodByMe.Core.Contracts.Data;

namespace FoodByMe.Core.Contracts
{
    public interface ISearchService : IReferenceBookService
    {
        List<Recipe> SearchRecipes(RecipeQuery query);
    }
}