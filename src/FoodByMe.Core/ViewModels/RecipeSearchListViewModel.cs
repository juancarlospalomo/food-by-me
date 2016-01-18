using System.Collections.ObjectModel;
using System.Windows.Input;
using FoodByMe.Core.Model;
using MvvmCross.Core.ViewModels;

namespace FoodByMe.Core.ViewModels
{
    public class RecipeSearchListViewModel : MvxViewModel
    {
        private ObservableCollection<RecipeListItemViewModel> _recipes;

        public RecipeSearchListViewModel()
        {
            _recipes = new ObservableCollection<RecipeListItemViewModel> {
                new RecipeListItemViewModel() { Category = RecipeCategory.Appetizer, Title = "Search 1"},
                new RecipeListItemViewModel() { Category = RecipeCategory.Soup, Title = "Search 2"}
            };
        }

        public ObservableCollection<RecipeListItemViewModel> Recipes
        {
            get { return _recipes; }
            set
            {
                _recipes = value;
                RaisePropertyChanged(() => Recipes);
            }
        }

        public ICommand ShowRecipeCommand => new MvxCommand<RecipeListItemViewModel>(ShowRecipe);

        private void ShowRecipe(RecipeListItemViewModel recipe)
        {
            ShowViewModel<RecipeDetailedListViewModel>();
        }
    }
}