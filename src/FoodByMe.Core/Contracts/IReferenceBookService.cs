using System.Collections.Generic;
using FoodByMe.Core.Contracts.Data;

namespace FoodByMe.Core.Contracts
{
    public interface IReferenceBookService
    {
        List<string> SearchIngredients(string query);

        RecipeCategory LookupCategory(int id);

        Measure LookupMeasure(int id);

        List<RecipeCategory> ListCategories();

        List<Measure> ListMeasures();
    }
}
