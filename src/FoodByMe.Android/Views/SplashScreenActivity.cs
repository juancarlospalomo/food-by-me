using Android.App;
using Android.Content.PM;
using FoodByMe.Core.Contracts;
using MvvmCross.Droid.Views;
using MvvmCross.Platform;

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

        public override void InitializationComplete()
        {
            var updateService = Mvx.Resolve<IUpdateService>();
            updateService.UpdateToLatestVersion();
            base.InitializationComplete();
        }
    }
}