using System;
using MvvmCross.Core.ViewModels;
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
    }
}