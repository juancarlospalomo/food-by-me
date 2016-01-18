using System.Collections.ObjectModel;
using MvvmCross.Core.ViewModels;

namespace FoodByMe.Core.ViewModels
{
    public class RecipeDetailedListViewModel : MvxViewModel
    {
        public ObservableCollection<RecipeDisplayViewModel> Recipes { get; }

        public RecipeDisplayViewModel SelectedRecipe { get; private set; }

        public RecipeDetailedListViewModel()
        {
            Recipes = new ObservableCollection<RecipeDisplayViewModel>();
        }

        public override void Start()
        {
            base.Start();
            Recipes.Clear();
            Recipes.Add(new RecipeDisplayViewModel());
            Recipes.Add(new RecipeDisplayViewModel());
            SelectedRecipe = new RecipeDisplayViewModel();
        }
    }
}