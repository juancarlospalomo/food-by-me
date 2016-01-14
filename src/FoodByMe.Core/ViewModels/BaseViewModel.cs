using Cirrious.MvvmCross.Localization;
using Cirrious.MvvmCross.ViewModels;

namespace FoodByMe.Core.ViewModels
{
    public class BaseViewModel : MvxViewModel
    {
        public IMvxLanguageBinder TextSource => new MvxLanguageBinder(GetType());
    }
}
