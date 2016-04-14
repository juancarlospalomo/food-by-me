using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using FoodByMe.Core.Contracts;
using FoodByMe.Core.Contracts.Data;
using FoodByMe.Core.Contracts.Messages;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Platform.Platform;
using MvvmCross.Plugins.Messenger;

namespace FoodByMe.Core.ViewModels
{
    public class RecipeDetailedListViewModel : BaseViewModel
    {
        private readonly IRecipeCollectionService _recipeService;
        private RecipeQuery _query;

        public ObservableCollection<RecipeDisplayViewModel> Recipes { get; private set; }

        public RecipeDisplayViewModel SelectedRecipe { get; private set; }

        public IMvxCommand DeleteCommand => new MvxAsyncCommand(Delete);

        public IMvxCommand EditCommand => new MvxCommand(Edit);

        private void Edit()
        {
            throw new System.NotImplementedException();
        }

        private async Task Delete()
        {
            await _recipeService.RemoveRecipeAsync(SelectedRecipe.Id);
            ShowViewModel<RecipeListViewModel>();
        }

        public int SelectedRecipeIndex => Recipes.IndexOf(SelectedRecipe);

        public RecipeDetailedListViewModel(IRecipeCollectionService recipeService)
        {
            if (recipeService == null)
            {
                throw new ArgumentNullException(nameof(recipeService));
            }
            _recipeService = recipeService;
            Recipes = new ObservableCollection<RecipeDisplayViewModel>();
        }

        public void Init(RecipeDetailedListParameters parameters)
        {
            Recipes.Clear();
            _query = new RecipeQuery
            {
                CategoryId = parameters.CategoryId,
                IsFavorite = parameters.IsFavorite,
                SearchTerm = parameters.SearchTerm
            };
            var recipes = _recipeService.SearchRecipesAsync(_query).Result;
            foreach (var recipe in recipes)
            {
                Recipes.Add(recipe.ToRecipeDisplayViewModel());
            }
            SelectedRecipe = Recipes.First(x => x.Id == parameters.RecipeId);
        }
    }
}