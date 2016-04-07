using MvvmCross.Plugins.Messenger;

namespace FoodByMe.Core.Contracts.Messages
{
    public class RecipeRemoved : MvxMessage
    {
        public RecipeRemoved(object sender, int recipeId) : base(sender)
        {
            RecipeId = recipeId;
        }

        public int RecipeId { get; }
    }
}
