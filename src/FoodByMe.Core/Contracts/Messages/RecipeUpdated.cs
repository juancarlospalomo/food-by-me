using FoodByMe.Core.Contracts.Data;
using MvvmCross.Plugins.Messenger;

namespace FoodByMe.Core.Contracts.Messages
{
    public class RecipeUpdated : MvxMessage
    {
        public RecipeUpdated(object sender, Recipe recipe) : base(sender)
        {
            Recipe = recipe;
        }

        public Recipe Recipe { get; }
    }
}
