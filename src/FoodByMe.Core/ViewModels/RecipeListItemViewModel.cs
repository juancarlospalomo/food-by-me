using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using FoodByMe.Core.Contracts.Data;
using FoodByMe.Core.Contracts.Messages;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;

namespace FoodByMe.Core.ViewModels
{
    public class RecipeListItemViewModel : BaseViewModel
    {
        private readonly IMvxMessenger _messenger;
        private bool _isFavorite;

        public static RecipeListItemViewModel Create(Recipe recipe, IMvxMessenger messenger)
        {
            return new RecipeListItemViewModel(messenger)
            {
                _isFavorite = recipe.IsFavorite,
                Id = recipe.Id,
                Title = recipe.Title,
                Description = recipe.Notes ?? GetDescriptionFromIngredients(recipe.Ingredients),
                CookingMinutes = recipe.CookingMinutes,
            };
        }

        private RecipeListItemViewModel(IMvxMessenger messenger)
        {
            _messenger = messenger;
            _messenger.Subscribe<RecipeFavoriteTagChanged>(OnFavoriteTagChanged);
        }

        internal int Id { get; private set; }

        public string Title { get; private set; }

        public string Description { get; private set; }

        public int CookingMinutes { get; private set; }

        public bool IsFavorite
        {
            get { return _isFavorite; }
            set
            {
                _isFavorite = value;
                RaisePropertyChanged(() => IsFavorite);
            }
        }

        public ICommand EditCommand => new MvxCommand(Edit);

        public ICommand ToggleFavoriteTagCommand => new MvxCommand(ToggleFavoriteTag);

        private void ToggleFavoriteTag()
        {
            var message = new RecipeFavoriteTagChanging(this, Id, IsFavorite);
            _messenger.Publish(message);
        }

        private void Edit()
        {
            ShowViewModel<RecipeEditViewModel>(new {recipeId = Id});
        }

        private void OnFavoriteTagChanged(RecipeFavoriteTagChanged message)
        {
            if (message.RecipeId == Id)
            {
                IsFavorite = message.IsFavorite;
            }
        }

        private static string GetDescriptionFromIngredients(IEnumerable<Ingredient> ingredients)
        {
            var texts = ingredients.Select(x => $"{x.Title} {x.Quantity}{x.Measure.ShortTitle}");
            return string.Join(", ", texts);
        }
    }
}
