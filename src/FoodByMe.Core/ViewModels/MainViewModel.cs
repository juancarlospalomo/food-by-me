using MvvmCross.Core.ViewModels;

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