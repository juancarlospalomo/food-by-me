using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FoodByMe.Core.Contracts;
using FoodByMe.Core.Contracts.Data;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;

namespace FoodByMe.Core.ViewModels
{
    public class RecipeCategoryMenuViewModel : MvxViewModel
    {
        private readonly IMvxMessenger _messenger;
        private readonly IReferenceBookService _referenceBook;

        public RecipeCategoryMenuViewModel(IMvxMessenger messenger, IReferenceBookService referenceBook)
        {
            if (messenger == null)
            {
                throw new ArgumentNullException(nameof(messenger));
            }
            if (referenceBook == null)
            {
                throw new ArgumentNullException(nameof(referenceBook));
            }
            _messenger = messenger;
            _referenceBook = referenceBook;
        }

        public List<RecipeCategory> Categories { get; private set; } 

        public IMvxCommand NavigateCommand => new MvxCommand<RecipeListParameters>(Navigate);

        public void Init()
        {
            Categories = _referenceBook.ListCategories();
        }

        private void Navigate(RecipeListParameters parameters)
        {
            ShowViewModel<RecipeListViewModel>(parameters);
        }
    }
}