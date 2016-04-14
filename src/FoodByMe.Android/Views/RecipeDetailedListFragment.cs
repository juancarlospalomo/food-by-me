using System.Collections.Specialized;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.View;
using Android.Support.V7.App;
using Android.Views;
using FoodByMe.Android.Framework;
using FoodByMe.Core.ViewModels;
using MvvmCross.Droid.Shared.Attributes;

namespace FoodByMe.Android.Views
{
    [MvxFragment(typeof(MainViewModel), Resource.Id.content_frame)]
    [Register("foodbyme.android.views.RecipeDetailedListFragment")]
    public class RecipeDetailedListFragment : ContentFragment<RecipeDetailedListViewModel>
    {
        private ViewPager _viewPager;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            HasOptionsMenu = true;
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            menu.Clear();
            inflater.Inflate(Resource.Menu.recipe_display_menu, menu);
            base.OnCreateOptionsMenu(menu, inflater);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);
            _viewPager = view.FindViewById<ViewPager>(Resource.Id.viewpager);
            _viewPager.Adapter = new ObservableCollectionFragmentStatePagerAdapter
                <RecipeDisplayFragment, RecipeDisplayViewModel>(
                ChildFragmentManager,
                ViewModel.Recipes,
                x => x.Title);
            _viewPager.PageSelected += OnPageSelected;
            _viewPager.CurrentItem = ViewModel.SelectedRecipeIndex;
            OnPageSelected(this, new ViewPager.PageSelectedEventArgs(_viewPager.CurrentItem));
            return view;
        }

        private void OnPageSelected(object sender, ViewPager.PageSelectedEventArgs e)
        {
            var actionBar = (Activity as AppCompatActivity)?.SupportActionBar;
            if (actionBar != null)
            {
                actionBar.Title = _viewPager.Adapter.GetPageTitle(e.Position);
            }
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.delete_recipe_menu_item:
                    ViewModel.DeleteCommand.Execute();
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }

        protected override int FragmentId => Resource.Layout.fragment_recipe_detailed_list;
    }
}