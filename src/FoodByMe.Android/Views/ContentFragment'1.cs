using MvvmCross.Core.ViewModels;

namespace FoodByMe.Android.Views
{
    public abstract class ContentFragment<TViewModel> : ContentFragment where TViewModel : class, IMvxViewModel
    {
        public new TViewModel ViewModel
        {
            get { return (TViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }
    }
}