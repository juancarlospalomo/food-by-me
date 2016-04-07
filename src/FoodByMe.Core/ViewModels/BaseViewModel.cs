using System;
using System.Collections.Generic;
using MvvmCross.Core.ViewModels;
using MvvmCross.Localization;

namespace FoodByMe.Core.ViewModels
{
    public class BaseViewModel : MvxViewModel
    {
        public IMvxLanguageBinder TextSource => new MvxLanguageBinder(GetType());

        protected BaseViewModel()
        {
            Subscriptions = new List<IDisposable>();
        }

        protected List<IDisposable> Subscriptions { get; } 
    }
}
