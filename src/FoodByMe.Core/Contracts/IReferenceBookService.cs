using System.Collections.Generic;
using System.Threading.Tasks;
using FoodByMe.Core.Contracts.Data;

namespace FoodByMe.Core.Contracts
{
    public interface IReferenceBookService
    {
        RecipeCategory LookupCategory(int id);

        Measure LookupMeasure(int id);

        Task<List<RecipeCategory>> ListCategoriesAsync();

        Task<List<Measure>> ListMeasuresAsync();
    }
}
