using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using FoodByMe.Core.Contracts;
using FoodByMe.Core.Contracts.Data;
using FoodByMe.Core.Contracts.Messages;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;

namespace FoodByMe.Core.ViewModels
{
    public class RecipeListViewModel : BaseViewModel
    {
        private RecipeQuery _query;
        private readonly IRecipeCollectionService _recipeService;
        private readonly IMvxMessenger _messenger;
        private ObservableCollection<RecipeListItemViewModel> _recipes;

        public RecipeListViewModel(IRecipeCollectionService recipeService, IMvxMessenger messenger)
        {
            if (recipeService == null)
            {
                throw new ArgumentNullException(nameof(recipeService));
            }
            if (messenger == null)
            {
                throw new ArgumentNullException(nameof(messenger));
            }
            _query = new RecipeQuery();
            _recipeService = recipeService;
            _messenger = messenger;
            Recipes = new ObservableCollection<RecipeListItemViewModel>();
            Subscriptions.AddRange(new List<IDisposable>
            {
                _messenger.Subscribe<RecipeAdded>(OnRecipeAdded),
                _messenger.Subscribe<RecipeFavoriteTagChanging>(OnRecipeFavoriteTagChanging),
                _messenger.Subscribe<RecipeFavoriteTagChanged>(OnRecipeFavoriteTagChanged),
                _messenger.Subscribe<RecipeUpdated>(OnRecipeUpdated),
                _messenger.Subscribe<RecipeRemoved>(OnRecipeRemoved),
            });
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

        public void Init(RecipeListParameters parameters)
        {
            _query = parameters == null ? new RecipeQuery() : new RecipeQuery
            {
                CategoryId = parameters.CategorySelected ? parameters.CategoryId : (int?) null,
                OnlyFavorite = parameters.IsFavoriteSelected,
                SearchTerm = parameters.SearchTerm
            };
            Refresh(_query);
            _messenger.Publish(new RecipeListLoaded(this, parameters));
        }

        private void ShowRecipe(RecipeListItemViewModel recipe)
        {
            ShowViewModel<RecipeDetailedListViewModel>(new RecipeDetailedListParameters
            {
                RecipeId = recipe.Id,
                CategoryId = _query.CategoryId ?? default(int),
                CategorySelected = _query.CategoryId.HasValue,
                IsFavoriteSelected = _query.OnlyFavorite,
                SearchTerm = _query.SearchTerm
            });
        }

        private void AddRecipe()
        {
            ShowViewModel<RecipeEditViewModel>(new RecipeEditParameters());
        }

        private void SearchRecipes(string query)
        {
            ShowViewModel<RecipeSearchListViewModel>();
        }

        private void OnRecipeFavoriteTagChanging(RecipeFavoriteTagChanging message)
        {
            _recipeService.ToggleRecipeFavoriteTagAsync(message.RecipeId, message.IsFavorite);
        }

        private void OnRecipeFavoriteTagChanged(RecipeFavoriteTagChanged message)
        {
        }

        private void OnRecipeAdded(RecipeAdded @event)
        {
            Recipes.Add(@event.Recipe.ToRecipeListItemViewModel(_messenger));
        }

        private void OnRecipeUpdated(RecipeUpdated @event)
        {
            var old = _recipes.FirstOrDefault(x => x.Id == @event.Recipe.Id);
            if (old == null)
            {
                return;
            }
            var index = _recipes.IndexOf(old);
            Recipes[index] = @event.Recipe.ToRecipeListItemViewModel(_messenger);
        }

        private void OnRecipeRemoved(RecipeRemoved @event)
        {
            var recipe = Recipes.FirstOrDefault(x => x.Id == @event.RecipeId);
            if (recipe != null)
            {
                Recipes.Remove(recipe);
            }
        }

        private async void Refresh(RecipeQuery query)
        {
            var recipes = await _recipeService.SearchRecipesAsync(query);
            var items = recipes.Select(x => x.ToRecipeListItemViewModel(_messenger));
            Recipes = new ObservableCollection<RecipeListItemViewModel>(items);            
        }
    }
}