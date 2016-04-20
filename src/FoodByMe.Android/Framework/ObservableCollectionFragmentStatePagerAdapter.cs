
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Android.App;
using Android.Support.V4.App;
using FoodByMe.Android.Views;
using Java.Lang;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.Core;
using Fragment = Android.Support.V4.App.Fragment;
using FragmentManager = Android.Support.V4.App.FragmentManager;

namespace FoodByMe.Android.Framework
{
    public class ObservableCollectionFragmentStatePagerAdapter<TFragment, TListItemViewModel> : FragmentPagerAdapter
        where TFragment : ContentFragment<TListItemViewModel>, new()
        where TListItemViewModel : class , IMvxViewModel
    {
        private readonly ObservableCollection<TListItemViewModel> _collection;
        private readonly Func<TListItemViewModel, string> _title;
        private readonly Dictionary<int, TFragment> _cache; 

        public ObservableCollectionFragmentStatePagerAdapter(
            FragmentManager fragmentManager,
            ObservableCollection<TListItemViewModel> collection,
            Func<TListItemViewModel, string> title) : base(fragmentManager)
        {
            _cache = new Dictionary<int, TFragment>();
            _collection = collection;
            _title = title;
            _collection.CollectionChanged += OnCollectionChanged;
        }

        public override int Count => _collection.Count;

        public override Fragment GetItem(int position)
        {
            if (_cache.ContainsKey(position))
            {
                return _cache[position];
            }
            var fragment = new TFragment {ViewModel = _collection[position]};
            _cache[position] = fragment;
            return fragment;
        }

        public override ICharSequence GetPageTitleFormatted(int position)
        {
            return new Java.Lang.String(_title(_collection[position]));
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            _cache.Clear();
            Application.SynchronizationContext.Post(x => NotifyDataSetChanged(), null);
        }
    }
}