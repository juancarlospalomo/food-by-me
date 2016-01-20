using FoodByMe.Core.Framework;
using FoodByMe.Core.Resources;
using FoodByMe.Core.Services;
using FoodByMe.Core.Services.Data;
using FoodByMe.Core.ViewModels;
using MvvmCross.Core.ViewModels;
using MvvmCross.Localization;
using MvvmCross.Platform;
using MvvmCross.Platform.IoC;

namespace FoodByMe.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            var textProvider = new ResxTextProvider(Text.ResourceManager);
            Mvx.RegisterSingleton<IMvxTextProvider>(textProvider);
            Mvx.RegisterSingleton<ICultureProvider>(textProvider);

            var dbSettings = new DatabaseSettings("FoodByMe.db3");
            Mvx.RegisterSingleton(dbSettings);

            RegisterAppStart<MainViewModel>();
        }
    }
}