using System;
using System.Globalization;
using MvvmCross.Localization;
using MvvmCross.Platform;
using MvvmCross.Platform.Converters;

namespace FoodByMe.Core.Framework
{
    public class CookingTimeValueConverter : MvxValueConverter<int, string>
    {
        private readonly Lazy<IMvxTextProvider> _textProvider = new Lazy<IMvxTextProvider>(Mvx.Resolve<IMvxTextProvider>);
        private const int MinutesInHour = 60;

        protected override string Convert(int value, Type targetType, object parameter, CultureInfo culture)
        {
            var hours = _textProvider.Value.GetText(null, null, "HoursShort");
            var minutes = _textProvider.Value.GetText(null, null, "MinutesShort");
            if (value < MinutesInHour)
            {
                return $"{value} {minutes}";
            }
            var mins = value % MinutesInHour;
            return mins == 0 
                ? $"{value / MinutesInHour} {hours}" 
                : $"{value / MinutesInHour} {hours} {mins} {minutes}";
        }
    }
}
