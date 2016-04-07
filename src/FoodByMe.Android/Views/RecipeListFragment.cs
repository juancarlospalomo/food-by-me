using System;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V7.Widget;
using Android.Views;
using FoodByMe.Core.ViewModels;
using MvvmCross.Droid.Shared.Attributes;
using MvvmCross.Droid.Support.V7.RecyclerView;
using SearchView = Android.Support.V7.Widget.SearchView;

namespace FoodByMe.Android.Views
{
    [MvxFragment(typeof(MainViewModel), Resource.Id.content_frame)]
    [Register("foodbyme.android.views.RecipeListFragment")]
    public class RecipeListFragment : ContentFragment<RecipeListViewModel>
    {
        private IDisposable _itemSelectedToken;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            HasOptionsMenu = true;
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            inflater.Inflate(Resource.Menu.recipe_list_menu, menu);
            var item = menu.FindItem(Resource.Id.search_recipes_menu_item);
            var searchView = (SearchView) MenuItemCompat.GetActionView(item);
            searchView.QueryTextSubmit += OnSearchSubmitted;

            base.OnCreateOptionsMenu(menu, inflater);
        }

        

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);
       
            var recyclerView = view.FindViewById<MvxRecyclerView>(Resource.Id.recipe_list_recycler_view);
            if (recyclerView != null)
            {
                recyclerView.HasFixedSize = true;
                var layoutManager = new LinearLayoutManager(Activity);
                recyclerView.SetLayoutManager(layoutManager);
            }

            //_itemSelectedToken = ViewModel.WeakSubscribe(() => ViewModel.SelectedItem,
            //    (sender, args) => {
            //        if (ViewModel.SelectedItem != null)
            //            Toast.MakeText(Activity,
            //                $"Selected: {ViewModel.SelectedItem.Title}",
            //                ToastLength.Short).Show();
            //    });

            //var swipeToRefresh = view.FindViewById<MvxSwipeRefreshLayout>(Resource.Id.refresher);
            //var appBar = Activity.FindViewById<AppBarLayout>(Resource.Id.appbar);
            //if (appBar != null)
            //    appBar.OffsetChanged += (sender, args) => swipeToRefresh.Enabled = args.VerticalOffset == 0;

            return view;
        }

        private void OnSearchSubmitted(object sender, SearchView.QueryTextSubmitEventArgs e)
        {
            ViewModel.SearchRecipesCommand.Execute(e.Query);
            e.Handled = true;
        }

        protected override int FragmentId => Resource.Layout.fragment_recipe_list;

        public override void OnDestroyView()
        {
            base.OnDestroyView();
            //_itemSelectedToken.Dispose();
            //_itemSelectedToken = null;
        }
    }
}