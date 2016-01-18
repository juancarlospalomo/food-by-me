using System.Collections.ObjectModel;
using System.Windows.Input;
using FoodByMe.Core.Model;
using MvvmCross.Core.ViewModels;

namespace FoodByMe.Core.ViewModels
{
    public class RecipeListViewModel : MvxViewModel
    {
        private ObservableCollection<RecipeListItemViewModel> _recipes;

        public RecipeListViewModel()
        {
            Recipes = new ObservableCollection<RecipeListItemViewModel> {
                new RecipeListItemViewModel() { Category = RecipeCategory.Appetizer, Title = "Pivo"},
                new RecipeListItemViewModel() { Category = RecipeCategory.Soup, Title = "Nyam"}
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

        public ICommand SearchRecipesCommand => new MvxCommand<string>(SearchRecipes);

        public ICommand ShowRecipeCommand => new MvxCommand<RecipeListItemViewModel>(ShowRecipe);

        public ICommand AddRecipeCommand => new MvxCommand(AddRecipe);

        private void ShowRecipe(RecipeListItemViewModel recipe)
        {
            ShowViewModel<RecipeDetailedListViewModel>();
        }

        private void AddRecipe()
        {
            ShowViewModel<RecipeEditViewModel>();
        }

        private void SearchRecipes(string query)
        {
            ShowViewModel<RecipeSearchListViewModel>();
        }
    }
}