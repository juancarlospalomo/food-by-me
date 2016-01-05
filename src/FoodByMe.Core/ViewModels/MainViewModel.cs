using Cirrious.MvvmCross.ViewModels;

namespace FoodByMe.Core.ViewModels
{
    public class MainViewModel : MvxViewModel
    {
        public MainViewModel()
        {
        }

        public void Load()
        {
            ShowViewModel<RecipeCategoryMenuViewModel>();
            ShowViewModel<RecipeListViewModel>();
        }
    }
}