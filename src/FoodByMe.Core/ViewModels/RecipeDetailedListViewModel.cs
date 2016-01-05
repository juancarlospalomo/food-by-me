using Cirrious.MvvmCross.ViewModels;

namespace FoodByMe.Core.ViewModels
{
    public class RecipeDetailedListViewModel : MvxViewModel
    {
        public RecipeDisplayViewModel Recycler { get; private set; }

        public RecipeDetailedListViewModel()
        {
            Recycler = new RecipeDisplayViewModel();
        }
    }
}