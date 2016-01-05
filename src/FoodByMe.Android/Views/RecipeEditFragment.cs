using Android.OS;
using Android.Runtime;
using Android.Views;
using FoodByMe.Core.ViewModels;
using MvvmCross.Droid.Support.V7.Fragging.Attributes;

namespace FoodByMe.Android.Views
{
    [MvxFragment(typeof(MainViewModel), Resource.Id.content_frame)]
    [Register("foodbyme.android.views.RecipeEditFragment")]
    public class RecipeEditFragment : ContentFragment<RecipeEditViewModel>
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            HasOptionsMenu = true;
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            base.OnCreateOptionsMenu(menu, inflater);
            inflater.Inflate(Resource.Menu.recipe_edit_menu, menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            return base.OnOptionsItemSelected(item);
        }

        protected override int FragmentId => Resource.Layout.fragment_recipe_edit;
    }
}