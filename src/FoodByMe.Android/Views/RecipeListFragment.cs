using System;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using FoodByMe.Core.Contracts;
using FoodByMe.Core.Contracts.Data;
using FoodByMe.Core.Resources;
using FoodByMe.Core.ViewModels;
using MvvmCross.Droid.Shared.Attributes;
using MvvmCross.Droid.Support.V7.RecyclerView;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using SearchView = Android.Support.V7.Widget.SearchView;

namespace FoodByMe.Android.Views
{
    [MvxFragment(typeof(MainViewModel), Resource.Id.content_frame)]
    [Register("foodbyme.android.views.RecipeListFragment")]
    public class RecipeListFragment : ContentFragment<RecipeListViewModel>
    {
        private readonly IReferenceBookService _referenceBook;

        public RecipeListFragment()
        {
            _referenceBook = Mvx.Resolve<IReferenceBookService>();
        }

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
            searchView.QueryTextChange += OnSearchChanged;

            base.OnCreateOptionsMenu(menu, inflater);
        }

        private void OnSearchChanged(object sender, SearchView.QueryTextChangeEventArgs e)
        {
            if (string.IsNullOrEmpty(e.NewText))
            {
                ViewModel.SearchRecipesCommand.Execute(e.NewText);
            }
            e.Handled = true;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);
            var recyclerView = view.FindViewById<MvxRecyclerView>(Resource.Id.recipe_list_recycler_view);
            Toolbar.Title = GetTitle(ViewModel.Query);
            if (recyclerView != null)
            {
                recyclerView.HasFixedSize = true;
                var layoutManager = new LinearLayoutManager(Activity);
                recyclerView.SetLayoutManager(layoutManager);
            }
            return view;
        }

        protected override int FragmentId => Resource.Layout.fragment_recipe_list;

        private void OnSearchSubmitted(object sender, SearchView.QueryTextSubmitEventArgs e)
        {
            ViewModel.SearchRecipesCommand.Execute(e.Query);
            Toolbar.Title = GetTitle(ViewModel.Query);
            e.Handled = true;
        }

        private string GetTitle(RecipeQuery query)
        {
            if (query.OnlyFavorite)
            {
                return Text.FavoritesLabel;
            }
            if (query.CategoryId != null)
            {
                var category = _referenceBook.LookupCategory(query.CategoryId.Value);
                return category.Title;
            }
            return Text.AllRecipesLabel;
        }
    }
}