using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;
using FoodByMe.Core.Contracts;
using FoodByMe.Core.Contracts.Data;
using FoodByMe.Core.Resources;
using FoodByMe.Core.ViewModels;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Shared.Attributes;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;


namespace FoodByMe.Android.Views
{
    [MvxFragment(typeof(MainViewModel), Resource.Id.navigation_frame)]
    [Register("foodbyme.android.views.SidebarFragment")]
    public class SidebarFragment : MvxFragment<RecipeCategoryMenuViewModel>
    {
        private static readonly Dictionary<int, int> CategoryIcons = new Dictionary<int, int>
        {
            [Constants.Categories.Appetizers] = Resource.Drawable.ic_app_appetizers,
            [Constants.Categories.Baking] = Resource.Drawable.ic_app_bakery,
            [Constants.Categories.Desserts] = Resource.Drawable.ic_app_desserts,
            [Constants.Categories.Dinner] = Resource.Drawable.ic_app_hot_dishes,
            [Constants.Categories.Drinks] = Resource.Drawable.ic_app_drinks,
            [Constants.Categories.Salads] = Resource.Drawable.ic_app_salads,
            [Constants.Categories.Other] = Resource.Drawable.ic_app_other
        };

        private readonly List<SidebarItem> _sidebarItems = new List<SidebarItem>();
        private NavigationView _navigationView;
        private IMenuItem _previousMenuItem;
        private readonly IMvxMessenger _messenger;
        private MvxSubscriptionToken _recipeListSubscription;

        public SidebarFragment()
        {
            _messenger = Mvx.Resolve<IMvxMessenger>();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            _recipeListSubscription = _messenger.SubscribeOnMainThread<RecipeListLoaded>(OnRecipeListLoaded);

            var view = this.BindingInflate(Resource.Layout.fragment_sidebar, null);

            _navigationView = view.FindViewById<NavigationView>(Resource.Id.navigation_view);
            _navigationView.NavigationItemSelected += OnNavigationItemSelected;

            _sidebarItems.Clear();
            var allRecipesItem = _navigationView.Menu.Add(0, 0, 0, Text.AllRecipesLabel);
            allRecipesItem.SetIcon(Resource.Drawable.ic_app_food);
            allRecipesItem.SetCheckable(true);
            allRecipesItem.SetChecked(true);
            _sidebarItems.Add(new SidebarItem
            {
                MenuItem = allRecipesItem
            });
            //((AppCompatActivity)Activity).SupportActionBar.Title = Text.AllRecipesLabel;

            var favoritesItem = _navigationView.Menu.Add(0, 1, 0, Text.FavoritesLabel);
            favoritesItem.SetIcon(Resource.Drawable.ic_favorite_black_24dp);
            _sidebarItems.Add(new SidebarItem
            {
                IsFavorite = true, 
                MenuItem = favoritesItem
            });

            var categoriesSubMenu = _navigationView.Menu.AddSubMenu(Text.CategoriesLabel);
            var itemId = 2;
            foreach (var category in ViewModel.Categories)
            {
                var categoryItem = categoriesSubMenu.Add(1, itemId++, 1, category.Title);
                categoryItem.SetIcon(CategoryIcons[category.Id]);
                _sidebarItems.Add(new SidebarItem
                {
                    Category = category,
                    MenuItem = categoryItem
                });
            }
            return view;
        }

        private void OnRecipeListLoaded(RecipeListLoaded @event)
        {
            if (!string.IsNullOrEmpty(@event.Parameters.SearchTerm))
            {
                _previousMenuItem?.SetChecked(false);
                return;
            }
            SidebarItem item;
            if (@event.Parameters.IsFavoriteSelected)
            {
                item = _sidebarItems.FirstOrDefault(x => x.IsFavorite);
            }
            else if (@event.Parameters.CategorySelected)
            {
                item = _sidebarItems.FirstOrDefault(x => x.Category?.Id == @event.Parameters.CategoryId);
            }
            else
            {
                item = _sidebarItems.FirstOrDefault(x => x.IsHome);
            }
            if (item != null)
            {
                ToggleMenuItem(item.MenuItem);
            }
        }

        private void ToggleMenuItem(IMenuItem item)
        {
            _previousMenuItem?.SetChecked(false);
            item.SetCheckable(true);
            item.SetChecked(true);
            
            _previousMenuItem = item;
        }

        private async void OnNavigationItemSelected(object sender, NavigationView.NavigationItemSelectedEventArgs e)
        {
            ToggleMenuItem(e.MenuItem);
            e.Handled = true;
            await Navigate(e.MenuItem.ItemId);
        }

        private async Task Navigate(int itemId)
        {
            ((MainActivity)Activity).DrawerLayout.CloseDrawers();
            await Task.Delay (TimeSpan.FromMilliseconds (250));

            RecipeListParameters listParams;
            var item = _sidebarItems.First(i => i.MenuItem.ItemId == itemId);

            if (item.Category != null)
            {
                listParams = new RecipeListParameters {CategoryId = item.Category.Id, CategorySelected = true};
            }
            else if (item.IsHome)
            {
                listParams = new RecipeListParameters();
            }
            else
            {
                listParams = new RecipeListParameters {IsFavoriteSelected = true};
            }
            ViewModel.NavigateCommand.Execute(listParams);
        }

        private class SidebarItem
        {
            public IMenuItem MenuItem { get; set; }

            public RecipeCategory Category { get; set; }

            public bool IsFavorite { get; set; }

            public bool IsHome => Category == null && !IsFavorite;
        }
    }
}