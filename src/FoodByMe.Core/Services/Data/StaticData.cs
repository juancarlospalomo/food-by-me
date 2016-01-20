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
                new RecipeCategory {Id = 1, Title = Text.ReferenceCategoryAppetizers},
                new RecipeCategory {Id = 2, Title = Text.ReferenceCategoryBaking},
                new RecipeCategory {Id = 3, Title = Text.ReferenceCategoryDesserts},
                new RecipeCategory {Id = 4, Title = Text.ReferenceCategoryDinner},
                new RecipeCategory {Id = 5, Title = Text.ReferenceCategoryDrinks},
                new RecipeCategory {Id = 6, Title = Text.ReferenceCategoryOther},
                new RecipeCategory {Id = 7, Title = Text.ReferenceCategorySalads}
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
                },
                new Measure
                {
                    Id = 2,
                    Title = Text.ReferenceMeasureGram,
                    ShortTitle = Text.ReferenceMeasureGramShort
                },
                new Measure
                {
                    Id = 3,
                    Title = Text.ReferenceMeasureKilogram,
                    ShortTitle = Text.ReferenceMeasureKilogramShort
                },
                new Measure
                {
                    Id = 4,
                    Title = Text.ReferenceMeasureLiter,
                    ShortTitle = Text.ReferenceMeasureLiterShort
                },
                new Measure
                {
                    Id = 5,
                    Title = Text.ReferenceMeasureMilligram,
                    ShortTitle = Text.ReferenceMeasureMilligramShort
                },
                new Measure
                {
                    Id = 6,
                    Title = Text.ReferenceMeasureMilliliter,
                    ShortTitle = Text.ReferenceMeasureMilliliterShort
                },
                new Measure
                {
                    Id = 7,
                    Title = Text.ReferenceMeasurePiece,
                    ShortTitle = Text.ReferenceMeasurePieceShort
                },
                new Measure
                {
                    Id = 8,
                    Title = Text.ReferenceMeasurePinch,
                    ShortTitle = Text.ReferenceMeasurePinchShort
                },
                new Measure
                {
                    Id = 9,
                    Title = Text.ReferenceMeasureTableSpoon,
                    ShortTitle = Text.ReferenceMeasureTableSpoonShort
                },
                new Measure
                {
                    Id = 10,
                    Title = Text.ReferenceMeasureTaste,
                    ShortTitle = Text.ReferenceMeasureTasteShort
                },
                new Measure
                {
                    Id = 11,
                    Title = Text.ReferenceMeasureTeaSpoon,
                    ShortTitle = Text.ReferenceMeasureTeaSpoonShort
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
