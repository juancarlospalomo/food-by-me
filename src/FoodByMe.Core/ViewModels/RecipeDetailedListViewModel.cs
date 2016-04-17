using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using FoodByMe.Core.Contracts;
using FoodByMe.Core.Contracts.Data;
using FoodByMe.Core.Contracts.Messages;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;

namespace FoodByMe.Core.ViewModels
{
    public class RecipeDetailedListViewModel : BaseViewModel
    {
        private readonly IRecipeCollectionService _recipeService;
        private RecipeQuery _query;

        public RecipeDetailedListViewModel(IRecipeCollectionService recipeService)
        {
            if (recipeService == null)
            {
                throw new ArgumentNullException(nameof(recipeService));
            }
            _recipeService = recipeService;
            Recipes = new ObservableCollection<RecipeDisplayViewModel>();
        }

        public ObservableCollection<RecipeDisplayViewModel> Recipes { get; }

        public int SelectedRecipeIndex { get; set; }

        public IMvxCommand DeleteCommand => new MvxAsyncCommand(Delete);

        public IMvxCommand EditCommand => new MvxCommand(Edit);

        public void Init(RecipeDetailedListParameters parameters)
        {
            Recipes.Clear();
            _query = new RecipeQuery
            {
                CategoryId = parameters.CategorySelected ? parameters.CategoryId : (int?)null,
                OnlyFavorite = parameters.IsFavoriteSelected,
                SearchTerm = parameters.SearchTerm
            };
            var recipes = _recipeService.SearchRecipesAsync(_query).Result;
            foreach (var recipe in recipes)
            {
                Recipes.Add(recipe.ToRecipeDisplayViewModel());
            }
            var selected = Recipes.First(x => x.Id == parameters.RecipeId);
            var index = Recipes.IndexOf(selected);
            SelectedRecipeIndex = index;
        }

        private void Edit()
        {
            var id = Recipes[SelectedRecipeIndex].Id;
            ShowViewModel<RecipeEditViewModel>(new RecipeEditParameters {RecipeId = id});
        }

        private async Task Delete()
        {
            var id = Recipes[SelectedRecipeIndex].Id;
            await _recipeService.RemoveRecipeAsync(id);
            ShowViewModel<RecipeListViewModel>();
        }
    }
}