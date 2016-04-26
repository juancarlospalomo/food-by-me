using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;
using FoodByMe.Core.Contracts;
using FoodByMe.Core.Contracts.Data;
using FoodByMe.Core.Contracts.Messages;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;

namespace FoodByMe.Core.ViewModels
{
    public class RecipeListViewModel : BaseViewModel
    {
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
            Query = new RecipeQuery();
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

        public bool IsEmpty => Recipes.Count == 0;

        public RecipeQuery Query { get; private set; }

        public ICommand SearchRecipesCommand => new MvxCommand<string>(SearchRecipes);

        public ICommand ShowRecipeCommand => new MvxCommand<RecipeListItemViewModel>(ShowRecipe);

        public ICommand AddRecipeCommand => new MvxCommand(AddRecipe);

        public void Init(RecipeListParameters parameters)
        {
            Query = parameters == null ? new RecipeQuery() : parameters.ToQuery();
            Refresh(Query);
        }

        public RecipeListParameters SaveState()
        {
            return Query.ToRecipeListParameters();
        }

        public void RestoreState(RecipeListParameters parameters)
        {
            Query = parameters == null ? new RecipeQuery() : parameters.ToQuery();
            Refresh(Query);
        }

        private void ShowRecipe(RecipeListItemViewModel recipe)
        {
            ShowViewModel<RecipeDetailedListViewModel>(new RecipeDetailedListParameters
            {
                RecipeId = recipe.Id,
                CategoryId = Query.CategoryId ?? default(int),
                CategorySelected = Query.CategoryId.HasValue,
                IsFavoriteSelected = Query.OnlyFavorite,
                SearchTerm = Query.SearchTerm
            });
        }

        private void AddRecipe()
        {
            ShowViewModel<RecipeEditViewModel>(new RecipeEditParameters {CategoryId = Query.CategoryId.GetValueOrDefault(0)});
        }

        private void SearchRecipes(string query)
        {
            Query = new RecipeQuery {SearchTerm = string.IsNullOrEmpty(query) ? null : query};
            Refresh(Query);
        }

        private void OnRecipeFavoriteTagChanging(RecipeFavoriteTagChanging message)
        {
            _recipeService.ToggleRecipeFavoriteTagAsync(message.RecipeId, message.IsFavorite);
        }

        private void OnRecipeFavoriteTagChanged(RecipeFavoriteTagChanged message)
        {
            if (!Query.OnlyFavorite)
            {
                return;
            }
            var recipe = Recipes.FirstOrDefault(x => x.Id == message.RecipeId);
            if (recipe != null)
            {
                Recipes.Remove(recipe);
            }
            if (Recipes.Count == 0)
            {
                RaisePropertyChanged(() => IsEmpty);
            }
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
            RaisePropertyChanged(() => IsEmpty);            
        }
    }
}