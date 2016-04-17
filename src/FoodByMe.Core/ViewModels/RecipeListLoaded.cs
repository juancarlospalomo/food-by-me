using MvvmCross.Plugins.Messenger;

namespace FoodByMe.Core.ViewModels
{
    public class RecipeListLoaded : MvxMessage
    {
        public RecipeListLoaded(object sender, RecipeListParameters parameters) : base(sender)
        {
            Parameters = parameters;
        }

        public RecipeListParameters Parameters { get; }
    }
}
