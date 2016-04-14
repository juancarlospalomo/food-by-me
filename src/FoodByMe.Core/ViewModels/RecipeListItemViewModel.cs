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

        public RecipeListItemViewModel(IMvxMessenger messenger)
        {
            _messenger = messenger;
            _messenger.Subscribe<RecipeFavoriteTagChanged>(OnFavoriteTagChanged);
        }

        internal int Id { get; set; }

        public string Title { get; set; }

        public string ImageUrl { get; set; }

        public string Description { get; set; }

        public int CookingMinutes { get; set; }

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
    }
}
