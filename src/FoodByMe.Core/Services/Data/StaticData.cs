using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using FoodByMe.Core.Contracts.Data;
using FoodByMe.Core.Resources;

namespace FoodByMe.Core.Services.Data
{
    internal static class StaticData
    {
        private const string MeasuresTag = "Categories";

        private const string CategoriesTag = "Categories";

        private static readonly Dictionary<string, object> Cache = new Dictionary<string, object>();

        public static Dictionary<int, RecipeCategory> Categories(CultureInfo culture)
        {
            if (culture == null)
            {
                throw new ArgumentNullException(nameof(culture));
            }
            return Load(CategoriesTag, ListCategories, x => x.Id, culture);
        }

        public static Dictionary<int, Measure> Measures(CultureInfo culture)
        {
            if (culture == null)
            {
                throw new ArgumentNullException(nameof(culture));
            }
            return Load(MeasuresTag, ListMeasures, x => x.Id, culture);

        }

        private static List<RecipeCategory> ListCategories()
        {
            var categories = new List<RecipeCategory>
            {
                new RecipeCategory {Id = 1, Title = Text.ReferenceCategoryAppetizers}
            };
            return categories;
        }

        private static List<Measure> ListMeasures()
        {
            var measures = new List<Measure>
            {
                new Measure
                {
                    Id = 1,
                    Title = Text.ReferenceMeasureCup,
                    ShortTitle = Text.ReferenceMeasureCupShort
                }
            };
            return measures;
        }

        private static Dictionary<TKey, T> Load<TKey, T>(string tag,
            Func<IEnumerable<T>> loader,
            Func<T, TKey> keyAccessor,
            CultureInfo culture)
        {
            var cacheKey = CacheKeyFor(tag, culture);
            if (Cache.ContainsKey(cacheKey))
            {
                return (Dictionary<TKey, T>)Cache[cacheKey];
            }
            var measures = new Dictionary<TKey, T>();
            WithCulture(culture, () =>
            {
                measures = loader().ToDictionary(keyAccessor);
                Cache[cacheKey] = measures;
            });
            return measures;
        } 

        private static void WithCulture(CultureInfo culture, Action action)
        {
            var previous = Text.Culture;
            Text.Culture = culture;
            action();
            Text.Culture = previous;
        }

        private static string CacheKeyFor(string tag, CultureInfo culture)
        {
            return $"{tag}-{culture}";
        }
    }
}
