using System;
using System.Threading.Tasks;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;
using FoodByMe.Core.ViewModels;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Support.V7.Fragging.Attributes;
using MvvmCross.Droid.Support.V7.Fragging.Fragments;

namespace FoodByMe.Android.Views
{
    [MvxFragment(typeof(MainViewModel), Resource.Id.navigation_frame)]
    [Register("foodbyme.android.views.SidebarFragment")]
    public class SidebarFragment : MvxFragment<RecipeCategoryMenuViewModel>, NavigationView.IOnNavigationItemSelectedListener
    {
        private NavigationView navigationView;
        private IMenuItem previousMenuItem;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var vm = base.ViewModel;
            var view = this.BindingInflate(Resource.Layout.fragment_sidebar, null);

            navigationView = view.FindViewById<NavigationView>(Resource.Id.navigation_view);
            navigationView.SetNavigationItemSelectedListener(this);
            //navigationView.Menu.FindItem(Resource.Id.nav_home).SetChecked(true);
            var item = navigationView.Menu.Add("All Recipes");
            item.SetIcon(Resource.Drawable.Icon);

            var categoriesSubMenu = navigationView.Menu.AddSubMenu("Measures");
            categoriesSubMenu.Add("Category 1");
            categoriesSubMenu.Add("Category 2");

            var tagsSubMenu = navigationView.Menu.AddSubMenu("Tags");
            tagsSubMenu.Add("Tag 1");
            tagsSubMenu.Add("Tag 2");

            return view;
        }

        public bool OnNavigationItemSelected(IMenuItem item)
        {
            item.SetCheckable(true);
            item.SetChecked(true);
            previousMenuItem?.SetChecked(false);
            previousMenuItem = item;

            Navigate(item.ItemId);

            return true;
        }

        private async Task Navigate(int itemId)
        {
            ((MainActivity)Activity).DrawerLayout.CloseDrawers ();
            await Task.Delay (TimeSpan.FromMilliseconds (250));

            switch (itemId) {
            //case Resource.Id.nav_viewpager:
            //        ViewModel.ChooseCategoryCommand.Execute(RecipeCategory.Bakery);
            //    break;
            //case Resource.Id.nav_recyclerview:
            //    break;
            //case Resource.Id.nav_settings:

            //    break;
            //case Resource.Id.nav_helpfeedback:
            //    break;
            }
        }
    }
}