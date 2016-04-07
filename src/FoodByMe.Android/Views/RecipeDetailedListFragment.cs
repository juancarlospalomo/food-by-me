using System.Collections.Generic;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.View;
using Android.Views;
using FoodByMe.Core.ViewModels;
using MvvmCross.Droid.Shared.Attributes;
using MvvmCross.Droid.Support.V4;

namespace FoodByMe.Android.Views
{
    [MvxFragment(typeof(MainViewModel), Resource.Id.content_frame)]
    [Register("foodbyme.android.views.RecipeDetailedListFragment")]
    public class RecipeDetailedListFragment : ContentFragment<RecipeDetailedListViewModel>
    {
        

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);

            var viewPager = view.FindViewById<ViewPager>(Resource.Id.viewpager);
            if (viewPager != null)
            {
                var fragments = new List<MvxFragmentPagerAdapter.FragmentInfo>
                {
                    new MvxFragmentPagerAdapter.FragmentInfo("RecyclerView 1", typeof (RecipeDisplayFragment),
                        typeof (RecipeDisplayViewModel)),
                    new MvxFragmentPagerAdapter.FragmentInfo("RecyclerView 2", typeof (RecipeDisplayFragment),
                        typeof (RecipeDisplayViewModel)),
                    new MvxFragmentPagerAdapter.FragmentInfo("RecyclerView 3", typeof (RecipeDisplayFragment),
                        typeof (RecipeDisplayViewModel)),
                    new MvxFragmentPagerAdapter.FragmentInfo("RecyclerView 4", typeof (RecipeDisplayFragment),
                        typeof (RecipeDisplayViewModel)),
                    new MvxFragmentPagerAdapter.FragmentInfo("RecyclerView 5", typeof (RecipeDisplayFragment),
                        typeof (RecipeDisplayViewModel))
                };
                viewPager.Adapter = new MvxFragmentPagerAdapter(Activity, ChildFragmentManager, fragments);
            }
            return view;
        }

        protected override int FragmentId => Resource.Layout.fragment_recipe_detailed_list;
    }
}