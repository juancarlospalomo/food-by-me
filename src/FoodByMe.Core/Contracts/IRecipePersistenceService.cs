using FoodByMe.Core.Contracts.Data;

namespace FoodByMe.Core.Contracts
{
    public interface IRecipePersistenceService
    {
        Recipe FindRecipe(int id);

        void RemoveRecipe(int id);

        Recipe SaveRecipe(Recipe recipe);
    }
}
