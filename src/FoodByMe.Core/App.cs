using FoodByMe.Core.Framework;
using FoodByMe.Core.Resources;
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

            Mvx.RegisterSingleton<IMvxTextProvider>(new ResxTextProvider(Text.ResourceManager));

            RegisterAppStart<MainViewModel>();
        }
    }
}