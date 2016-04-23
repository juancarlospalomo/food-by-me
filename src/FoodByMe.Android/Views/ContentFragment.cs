using Android.Content.Res;
using Android.OS;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using FoodByMe.Core.Resources;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Droid.Support.V7.AppCompat;

namespace FoodByMe.Android.Views
{
    public abstract class ContentFragment : MvxFragment, View.IOnClickListener
    {
        protected Toolbar Toolbar;

        public MvxActionBarDrawerToggle DrawerToggle;

        protected ContentFragment()
        {
            this.RetainInstance = true;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(FragmentId, null);
            Toolbar = view.FindViewById<Toolbar>(Resource.Id.toolbar);
            var main = (MainActivity) Activity;

            if (Toolbar == null)
            {
                return view;
            }
            main.SetSupportActionBar(Toolbar);
            Toolbar.Title = Text.NewRecipeTitle;
            main.SupportActionBar.Title = Text.NewRecipeTitle;
            main.SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            DrawerToggle = new MvxActionBarDrawerToggle(
                Activity, // host Activity
                main.DrawerLayout, // DrawerLayout object
                Toolbar, // nav drawer icon to replace 'Up' caret
                Resource.String.drawer_open, // "open drawer" description
                Resource.String.drawer_close) // "close drawer" description
            {
                ToolbarNavigationClickListener = this
            };
            var drawerLayout = ((MainActivity) Activity).DrawerLayout;
            drawerLayout?.SetDrawerListener(DrawerToggle);
            return view;
        }

        protected abstract int FragmentId { get; }

        protected ActionBar ActionBar => ((MainActivity) Activity).SupportActionBar;

        public override void OnConfigurationChanged(Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);
            if (Toolbar != null)
            {
                DrawerToggle.OnConfigurationChanged(newConfig);
            }
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);
            if (Toolbar != null)
            {
                DrawerToggle.SyncState();
            }
        }

        public void OnClick(View v)
        {
            Activity.OnBackPressed();
        }
    }
}

