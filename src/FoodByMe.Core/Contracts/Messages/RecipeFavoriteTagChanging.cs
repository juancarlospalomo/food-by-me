using MvvmCross.Plugins.Messenger;

namespace FoodByMe.Core.Contracts.Messages
{
    public class RecipeFavoriteTagChanging : MvxMessage
    {
        public RecipeFavoriteTagChanging(object sender, int recipeId, bool isFavorite) : base(sender)
        {
            RecipeId = recipeId;
            IsFavorite = isFavorite;
        }

        public int RecipeId { get; }

        public bool IsFavorite { get; }
    }
}
