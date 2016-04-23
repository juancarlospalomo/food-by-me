using System;
using MvvmCross.Droid.Shared.Caching;

namespace FoodByMe.Android.Framework.Caching
{
    public class CustomFragmentInfo : MvxCachedFragmentInfo
    {
        public CustomFragmentInfo(string tag, Type fragmentType, Type viewModelType, bool cacheFragment,
            bool addToBackstack = false,
            bool isRoot = false)
            : base(tag, fragmentType, viewModelType, cacheFragment, addToBackstack)
        {
            IsRoot = isRoot;
        }

        public bool IsRoot { get; set; }
    }
}