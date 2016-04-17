using System;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;

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
            ShowViewModel<RecipeListViewModel>(new RecipeListParameters());
        }
    }
}