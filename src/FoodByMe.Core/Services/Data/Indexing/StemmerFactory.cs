using System;
using System.Globalization;
using FoodByMe.Core.Services.Data.Indexing.Stemmers;

namespace FoodByMe.Core.Services.Data.Indexing
{
    internal class StemmerFactory
    {
        public static IStemmer Create(CultureInfo culture)
        {
            if (culture == null)
            {
                throw new ArgumentNullException(nameof(culture));
            }
            switch (culture.TwoLetterISOLanguageName)
            {
                case "ru":
                    return new RussianStemmer();
                default:
                    return new EnglishStemmer();
            }
        }
    }
}
