using System;
using System.ComponentModel;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using FoodByMe.Core.Resources;
using FoodByMe.Core.ViewModels;
using MvvmCross.Droid.Shared.Attributes;

namespace FoodByMe.Android.Views
{
    [MvxFragment(typeof(MainViewModel), Resource.Id.content_frame)]
    [Register("foodbyme.android.views.RecipeEditFragment")]
    public class RecipeEditFragment : ContentFragment<RecipeEditViewModel>
    {
        private const int PickImageId = 1000;

        private CollapsingToolbarLayout _toolbarLayout;
        private FloatingActionButton _chooseImageButton;

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

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);
            _toolbarLayout = view.FindViewById<CollapsingToolbarLayout>(Resource.Id.collapsing_toolbar);
            _chooseImageButton = view.FindViewById<FloatingActionButton>(Resource.Id.choose_photo_button);
            _chooseImageButton.Click += OnChooseImageButtonClick;
            ViewModel.PropertyChanged += OnViewModelPropertyChanged;
            return view;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            ViewModel.PropertyChanged -= OnViewModelPropertyChanged;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            return base.OnOptionsItemSelected(item);
        }

        protected override int FragmentId => Resource.Layout.fragment_recipe_edit;

        public override void OnActivityResult(int requestCode, int resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Title")
            {
                _toolbarLayout.Title = ViewModel.Title;
            }
        }

        private void OnChooseImageButtonClick(object sender, EventArgs e)
        {
            var options = new []
            {
                Text.PickPhotoGallery,
                Text.PickPhotoCamera,
                Text.Cancel
            };
            using (var builder = new AlertDialog.Builder(Context))
            {
                builder.SetTitle(Text.PickPhoto);
                builder.SetItems(options, OnPhotoPickerChoice);
                builder.Show();
            }
        }

        private void OnPhotoPickerChoice(object sender, DialogClickEventArgs e)
        {
            switch (e.Which)
            {
                case 0:
                    ViewModel.ChoosePhotoCommand.Execute(null);
                    break;
                case 1:
                    ViewModel.TakePhotoCommand.Execute(null);
                    break;
            }
        }
    }
}