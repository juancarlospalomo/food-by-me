using System;
using Cirrious.MvvmCross.ViewModels;

namespace FoodByMe.Core.ViewModels
{
    public class RecipeDisplayViewModel : MvxViewModel
    {
        private string _title;

        public RecipeDisplayViewModel()
        {
            var r = new Random();
            _title = $"Recipe ${r.Next()}";
        }

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                RaisePropertyChanged(() => Title);
            }
        }
    }
}