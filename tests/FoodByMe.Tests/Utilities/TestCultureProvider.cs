using System.Globalization;
using FoodByMe.Core.Framework;

namespace FoodByMe.Tests.Utilities
{
    public class TestCultureProvider : ICultureProvider
    {
        public TestCultureProvider(CultureInfo culture)
        {
            Culture = culture;
        }

        public CultureInfo Culture { get; }
    }
}
