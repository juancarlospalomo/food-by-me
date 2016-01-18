using Android.App;
using Android.Content.PM;
using MvvmCross.Droid.Views;

namespace FoodByMe.Android.Views
{
    [Activity(
        Label = "FoodByMe", 
        MainLauncher = true, 
        Icon = "@drawable/icon", 
        Theme = "@style/AppTheme.Splash",
        NoHistory = true, 
        ScreenOrientation = ScreenOrientation.Portrait
    )]
    public class SplashScreenActivity : MvxSplashScreenActivity
    {
        public SplashScreenActivity() : base(Resource.Layout.activity_splash_screen)
        {
        }
    }
}