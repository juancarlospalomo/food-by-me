using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Views;
using FoodByMe.Android.Framework.Caching;
using FoodByMe.Core.ViewModels;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Droid.Support.V7.Fragging.Caching;
using FragmentTransaction = Android.Support.V4.App.FragmentTransaction;

namespace FoodByMe.Android.Views
{
    [Activity(
        Label = "Recipes",
        Theme = "@style/AppTheme",
        LaunchMode = LaunchMode.SingleTop,
        Name = "foodbyme.android.views.MainActivity"
    )]
    public class MainActivity : MvxCachingFragmentCompatActivity<MainViewModel>
    {
        public DrawerLayout DrawerLayout;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.activity_main);

            DrawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);

            if (bundle == null)
                ViewModel.Load();
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case global::Android.Resource.Id.Home:
                    DrawerLayout.OpenDrawer(GravityCompat.Start);
                    return true;
            }
            
            return base.OnOptionsItemSelected(item);
        }

        public override IFragmentCacheConfiguration BuildFragmentCacheConfiguration()
        {
            return new FragmentCacheConfigurationCustomFragmentInfo(); // custom FragmentCacheConfiguration is used because custom IMvxFragmentInfo is used -> CustomFragmentInfo
        }

        public override void OnFragmentCreated(IMvxCachedFragmentInfo fragmentInfo, FragmentTransaction transaction)
        {
            base.OnFragmentCreated(fragmentInfo, transaction);

            var myCustomInfo = fragmentInfo as CustomFragmentInfo;

            // You can do fragment + transaction based configurations here.
            // Note that, the cached fragment might be reused in another transaction afterwards.
        }

        public override void OnFragmentPopped(IList<IMvxCachedFragmentInfo> currentFragmentsInfo)
        {
            base.OnFragmentPopped(currentFragmentsInfo);
            OnFragmentChanged(currentFragmentsInfo.Last());
        }

        private void CheckIfMenuIsNeeded(CustomFragmentInfo myCustomInfo)
        {
            //If not root, we will block the menu sliding gesture and show the back button on top
            if (myCustomInfo == null || myCustomInfo.IsRoot)
                ShowHamburguerMenu();
            else
                ShowBackButton();
        }

        private void ShowBackButton()
        {
            //TODO Tell the toggle to set the indicator off
            //this.DrawerToggle.DrawerIndicatorEnabled = false;

            var fr = SupportFragmentManager.FindFragmentById(Resource.Id.content_frame) as ContentFragment;
            if (fr?.DrawerToggle != null)
            {
                fr.DrawerToggle.DrawerIndicatorEnabled = false;
            }

            //Block the menu slide gesture
            DrawerLayout.SetDrawerLockMode(DrawerLayout.LockModeLockedClosed);
        }

        private void ShowHamburguerMenu()
        {
            //TODO set toggle indicator as enabled 
            //this.DrawerToggle.DrawerIndicatorEnabled = true;

            var fr = SupportFragmentManager.FindFragmentById(Resource.Id.content_frame) as ContentFragment;
            if (fr?.DrawerToggle != null)
            {
                fr.DrawerToggle.DrawerIndicatorEnabled = true;
            }

            //Unlock the menu sliding gesture
            DrawerLayout.SetDrawerLockMode(DrawerLayout.LockModeUnlocked);
        }


        public override void OnFragmentChanged(IMvxCachedFragmentInfo fragmentInfo)
        {
            var myCustomInfo = fragmentInfo as CustomFragmentInfo;
            CheckIfMenuIsNeeded(myCustomInfo);
        }


        public override void OnBackPressed()
        {
            if (DrawerLayout != null && DrawerLayout.IsDrawerOpen(GravityCompat.Start))
                DrawerLayout.CloseDrawers();
            else
            {
                base.OnBackPressed();
            }
        }
    }

    public class CustomFragmentInfo : MvxCachedFragmentInfo
    {
        public CustomFragmentInfo(string tag, Type fragmentType, Type viewModelType, bool cacheFragment, bool addToBackstack = false,
            bool isRoot = false)
            : base(tag, fragmentType, viewModelType, cacheFragment, addToBackstack)
        {
            IsRoot = isRoot;
        }

        public bool IsRoot { get; set; }
    }
}