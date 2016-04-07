using MvvmCross.Localization;

namespace FoodByMe.Core.Framework
{
    public static class TextProviderExtensions
    {
        public static string GetText(this IMvxTextProvider textProvider, string key, params object[] format)
        {
            return textProvider.GetText(null, null, key, format);
        }
    }
}
