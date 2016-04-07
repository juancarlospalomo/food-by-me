using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using FoodByMe.Core.Contracts;
using FoodByMe.Core.Contracts.Data;
using FoodByMe.Core.Resources;

namespace FoodByMe.Core.Services.Data
{
    internal static class StaticData
    {
        private const string MeasuresTag = "Measures";

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
                new RecipeCategory {Id = Constants.Categories.Appetizers, Title = Text.ReferenceCategoryAppetizers},
                new RecipeCategory {Id = Constants.Categories.Baking, Title = Text.ReferenceCategoryBaking},
                new RecipeCategory {Id = Constants.Categories.Desserts, Title = Text.ReferenceCategoryDesserts},
                new RecipeCategory {Id = Constants.Categories.Dinner, Title = Text.ReferenceCategoryDinner},
                new RecipeCategory {Id = Constants.Categories.Drinks, Title = Text.ReferenceCategoryDrinks},
                new RecipeCategory {Id = Constants.Categories.Other, Title = Text.ReferenceCategoryOther},
                new RecipeCategory {Id = Constants.Categories.Salads, Title = Text.ReferenceCategorySalads}
            };
            return categories;
        }

        private static List<Measure> ListMeasures()
        {
            var measures = new List<Measure>
            {
                new Measure
                {
                    Id = Constants.Measures.Cup,
                    Title = Text.ReferenceMeasureCup,
                    ShortTitle = Text.ReferenceMeasureCupShort
                },
                new Measure
                {
                    Id = Constants.Measures.Gram,
                    Title = Text.ReferenceMeasureGram,
                    ShortTitle = Text.ReferenceMeasureGramShort
                },
                new Measure
                {
                    Id = Constants.Measures.Kilogram,
                    Title = Text.ReferenceMeasureKilogram,
                    ShortTitle = Text.ReferenceMeasureKilogramShort
                },
                new Measure
                {
                    Id = Constants.Measures.Liter,
                    Title = Text.ReferenceMeasureLiter,
                    ShortTitle = Text.ReferenceMeasureLiterShort
                },
                new Measure
                {
                    Id = Constants.Measures.Milligram,
                    Title = Text.ReferenceMeasureMilligram,
                    ShortTitle = Text.ReferenceMeasureMilligramShort
                },
                new Measure
                {
                    Id = Constants.Measures.Milliliter,
                    Title = Text.ReferenceMeasureMilliliter,
                    ShortTitle = Text.ReferenceMeasureMilliliterShort
                },
                new Measure
                {
                    Id = Constants.Measures.Piece,
                    Title = Text.ReferenceMeasurePiece,
                    ShortTitle = Text.ReferenceMeasurePieceShort
                },
                new Measure
                {
                    Id = Constants.Measures.Pinch,
                    Title = Text.ReferenceMeasurePinch,
                    ShortTitle = Text.ReferenceMeasurePinchShort
                },
                new Measure
                {
                    Id = Constants.Measures.TableSpoon,
                    Title = Text.ReferenceMeasureTableSpoon,
                    ShortTitle = Text.ReferenceMeasureTableSpoonShort
                },
                new Measure
                {
                    Id = Constants.Measures.TeaSpoon,
                    Title = Text.ReferenceMeasureTeaSpoon,
                    ShortTitle = Text.ReferenceMeasureTeaSpoonShort
                },
                new Measure
                {
                    Id = Constants.Measures.Taste,
                    Title = Text.ReferenceMeasureTaste,
                    ShortTitle = Text.ReferenceMeasureTasteShort
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
