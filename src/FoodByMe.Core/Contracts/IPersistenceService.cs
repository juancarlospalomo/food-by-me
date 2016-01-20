using System.Threading.Tasks;
using FoodByMe.Core.Contracts.Data;

namespace FoodByMe.Core.Contracts
{
    public interface IPersistenceService
    {
        Task<Recipe> SaveRecipeAsync(Recipe recipe);
    }
}
