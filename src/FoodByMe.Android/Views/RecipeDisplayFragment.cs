using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using FoodByMe.Core.ViewModels;
using MvvmCross.Droid.Support.V7.Fragging.Attributes;

namespace FoodByMe.Android.Views
{
    [MvxFragment(typeof(RecipeDisplayViewModel), Resource.Id.content_frame)]
    [Register("foodbyme.android.views.RecipeDisplayFragment")]
    public class RecipeDisplayFragment : ContentFragment<RecipeDisplayViewModel>
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            HasOptionsMenu = true;
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            inflater.Inflate(Resource.Menu.recipe_display_menu, menu);
            base.OnCreateOptionsMenu(menu, inflater);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            return base.OnOptionsItemSelected(item);
        }

        protected override int FragmentId => Resource.Layout.fragment_recipe_display;
    }
}