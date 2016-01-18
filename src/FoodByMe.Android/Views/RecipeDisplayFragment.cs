using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Support.V7.Graphics;
using Android.Views;
using Android.Widget;
using FoodByMe.Core.ViewModels;
using MvvmCross.Binding.Droid.Views;
using MvvmCross.Droid.Support.V7.Fragging.Attributes;
using MvvmCross.Platform;
using MvvmCross.Platform.Platform;

namespace FoodByMe.Android.Views
{
    [MvxFragment(typeof(RecipeDisplayViewModel), Resource.Id.content_frame)]
    [Register("foodbyme.android.views.RecipeDisplayFragment")]
    public class RecipeDisplayFragment : ContentFragment<RecipeDisplayViewModel>
    {

        public RecipeDisplayFragment()
        {
        }

        public int AdjustAlpha(int color, float factor)
        {
            int alpha = (int) Math.Round(Color.GetAlphaComponent(color) * factor);
            int red = Color.GetRedComponent(color);
            int green = Color.GetGreenComponent(color);
            int blue = Color.GetBlueComponent(color);
            return Color.Argb(alpha, red, green, blue);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            Mvx.Trace(MvxTraceLevel.Warning, "Creating view");
            var view = base.OnCreateView(inflater, container, savedInstanceState);
            var imageView = view.FindViewById<MvxImageView>(Resource.Id.display_recipe_image_iv);
            var titleTextView = view.FindViewById<TextView>(Resource.Id.display_recipe_title_tv);
            var categoryTextView = view.FindViewById<TextView>(Resource.Id.display_recipe_category_tv);
            var layout = view.FindViewById<LinearLayout>(Resource.Id.recipe_display_title_layout);
            var activity = Activity as AppCompatActivity;
            if (activity?.SupportActionBar != null)
            {
                activity.SupportActionBar.Title = "Пирог с яблоками и желтым лимоном";
            }
            using (var bitmap = BitmapFactory.DecodeResource(Resources, Resource.Drawable.cake))
            using (var palette = Palette.From(bitmap).MaximumColorCount(16).Generate())
            {
                var back = new Color(palette.VibrantSwatch.Rgb);
                back = new Color(AdjustAlpha(back, 0.5f));
                imageView.SetImageBitmap(bitmap);
                layout.SetBackgroundColor(new Color(back));
                titleTextView.SetTextColor(new Color(palette.VibrantSwatch.TitleTextColor));
                categoryTextView.SetTextColor(new Color(palette.VibrantSwatch.BodyTextColor));
            }

            return view;
        }

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

        public override void OnStart()
        {
            base.OnStart();
            
        }

        protected override int FragmentId => Resource.Layout.fragment_recipe_display;
    }
}