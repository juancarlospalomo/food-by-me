using FoodByMe.Core.Model;
using MvvmCross.Plugins.Messenger;

namespace FoodByMe.Core.Messages
{
    public class CategoryChosenMessage : MvxMessage
    {
        public CategoryChosenMessage(object sender, RecipeCategory category) : base(sender)
        {
            Category = category;
        }

        public RecipeCategory Category { get; }
    }
}
