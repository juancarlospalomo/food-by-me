using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
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

        public override void Start()
        {
            Refresh(_query);
        }

        protected override void InitFromBundle(IMvxBundle parameters)
        {
            base.InitFromBundle(parameters);
        }

        protected override void SaveStateToBundle(IMvxBundle bundle)
        {
            base.SaveStateToBundle(bundle);
        }

        protected override void ReloadFromBundle(IMvxBundle state)
        {
            base.ReloadFromBundle(state);
        }

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

        private void OnRecipeFavoriteTagChanging(RecipeFavoriteTagChanging message)
        {
            _recipeService.ToggleRecipeFavoriteTagAsync(message.RecipeId, message.IsFavorite);
        }

        private void OnRecipeFavoriteTagChanged(RecipeFavoriteTagChanged message)
        {
        }

        private void OnRecipeAdded(RecipeAdded recipe)
        {
            
        }

        private void OnRecipeUpdated(RecipeUpdated obj)
        {
        }

        private void OnRecipeRemoved(RecipeRemoved obj)
        {
        }

        private async void Refresh(RecipeQuery query)
        {
            var recipes = await _recipeService.SearchRecipesAsync(query);
            var items = recipes.Select(x => RecipeListItemViewModel.Create(x, _messenger));
            Recipes = new ObservableCollection<RecipeListItemViewModel>(items);            
        }
    }
}