using FoodByMe.Core.Model;
using MvvmCross.Core.ViewModels;

namespace FoodByMe.Core.ViewModels
{
    public class RecipeListItemViewModel : MvxViewModel
    {
        public RecipeListItemViewModel()
        {
        }

        public string Title { get; set; }

        public RecipeCategory Category { get; set; }
    }
}
