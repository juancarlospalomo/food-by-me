using Cirrious.CrossCore;
using Cirrious.CrossCore.IoC;
using Cirrious.MvvmCross.Localization;
using FoodByMe.Core.Framework;
using FoodByMe.Core.Resources;
using FoodByMe.Core.ViewModels;
using Microsoft.VisualBasic;

namespace FoodByMe.Core
{
    public class App : Cirrious.MvvmCross.ViewModels.MvxApplication
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