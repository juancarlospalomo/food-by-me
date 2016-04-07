using MvvmCross.Plugins.Messenger;

namespace FoodByMe.Core.Contracts.Messages
{
    public class RecipeFavoriteTagChanged : MvxMessage
    {
        public RecipeFavoriteTagChanged(object sender, int recipeId, bool isFavorite) : base(sender)
        {
            RecipeId = recipeId;
            IsFavorite = isFavorite;
        }

        public int RecipeId { get; }

        public bool IsFavorite { get; }
    }
}
