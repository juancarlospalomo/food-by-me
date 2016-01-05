using Cirrious.MvvmCross.ViewModels;
using FoodByMe.Core.Model;

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
