using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Threading;
using Android.Content;
using Cirrious.CrossCore.Converters;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Droid.Platform;
using Cirrious.MvvmCross.Droid.Views;
using Cirrious.MvvmCross.Localization;
using Cirrious.MvvmCross.ViewModels;
using FoodByMe.Android.Framework;
using MvvmCross.Droid.Support.V7.Fragging.Presenter;

namespace FoodByMe.Android
{
    public class Setup : MvxAndroidSetup
    {
        public Setup(Context applicationContext)
            : base(applicationContext)
        {
        }

        protected override IMvxApplication CreateApp()
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("ru-RU");
            return new Core.App();
        }

		protected override IEnumerable<Assembly> AndroidViewAssemblies => new List<Assembly>(base.AndroidViewAssemblies)
		{
			typeof(global::Android.Support.Design.Widget.NavigationView).Assembly,
			typeof(global::Android.Support.Design.Widget.FloatingActionButton).Assembly,
			typeof(global::Android.Support.V7.Widget.Toolbar).Assembly,
			typeof(global::Android.Support.V4.Widget.DrawerLayout).Assembly,
			typeof(global::Android.Support.V4.View.ViewPager).Assembly,
			typeof(MvvmCross.Droid.Support.V7.RecyclerView.MvxRecyclerView).Assembly
		};

        protected override IMvxAndroidViewPresenter CreateViewPresenter()
        {
			return new MvxFragmentsPresenter(AndroidViewAssemblies);
        }

        protected override IMvxTrace CreateDebugTrace()
        {
            return new DebugTrace();
        }

        protected override void FillValueConverters(IMvxValueConverterRegistry registry)
        {
            base.FillValueConverters(registry);
            registry.AddOrOverwrite("Language", new MvxLanguageConverter());
        }
    }
}