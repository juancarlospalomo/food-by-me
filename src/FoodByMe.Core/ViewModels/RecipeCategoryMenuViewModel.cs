using System;
using System.Windows.Input;
using Cirrious.MvvmCross.ViewModels;
using FoodByMe.Core.Messages;
using FoodByMe.Core.Model;
using MvvmCross.Plugins.Messenger;

namespace FoodByMe.Core.ViewModels
{
    public class RecipeCategoryMenuViewModel : MvxViewModel
    {
        private readonly IMvxMessenger _messenger;

        public RecipeCategoryMenuViewModel(IMvxMessenger messenger)
        {
            if (messenger == null)
            {
                throw new ArgumentNullException(nameof(messenger));
            }
            _messenger = messenger;
        }

        public ICommand ChooseCategoryCommand => new MvxCommand<RecipeCategory>(ChooseCategory);

        private void ChooseCategory(RecipeCategory category)
        {
            var message = new CategoryChosenMessage(this, category);
            _messenger.Publish(message);
        }
    }
}