using System;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using FoodByMe.Core.Contracts;
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
        private MvxSubscriptionToken _recipeListLoadedToken;
        private readonly IReferenceBookService _referenceBook;

        public RecipeListFragment()
        {
            var messenger = Mvx.Resolve<IMvxMessenger>();
            _referenceBook = Mvx.Resolve<IReferenceBookService>();
            _recipeListLoadedToken = messenger.Subscribe<RecipeListLoaded>(OnRecipeListLoaded);
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
            return view;
        }

        protected override int FragmentId => Resource.Layout.fragment_recipe_list;

        private void OnSearchSubmitted(object sender, SearchView.QueryTextSubmitEventArgs e)
        {
            ViewModel.SearchRecipesCommand.Execute(e.Query);
            e.Handled = true;
        }

        private void OnRecipeListLoaded(RecipeListLoaded @event)
        {
            var compatActivity = (AppCompatActivity) Activity;
            if (compatActivity.SupportActionBar == null)
            {
                return;
            }
            if (@event.Parameters.IsFavoriteSelected)
            {
                compatActivity.SupportActionBar.Title = Text.FavoritesLabel;
            }
            else if (@event.Parameters.CategorySelected)
            {
                var category = _referenceBook.LookupCategory(@event.Parameters.CategoryId);
                compatActivity.SupportActionBar.Title = category.Title;
            }
            else
            {
                compatActivity.SupportActionBar.Title = Text.AllRecipesLabel;
            }
        }
    }
}