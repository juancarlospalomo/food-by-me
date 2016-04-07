using System;
using System.Collections.Generic;
using FoodByMe.Core.Contracts;
using FoodByMe.Core.Contracts.Data;
using FoodByMe.Core.Services.Data;

namespace FoodByMe.Core.Services.Updates
{
    [Update(2, "Initial set of recipes")]
    internal class DefaultRecipesUpdate : IUpdate
    {
        private static Dictionary<string, Func<IReferenceBookService, IEnumerable<Recipe>>> _recipes =
            new Dictionary<string, Func<IReferenceBookService, IEnumerable<Recipe>>>
        {
                {"ru", Russian},
                {"en", English}

        }; 

        public void Apply(UpdateContext context)
        {
            var service = context.RecipePersistenceService;
            var lang = context.CultureProvider.Culture.TwoLetterISOLanguageName;
            if (!_recipes.ContainsKey(lang))
            {
                return;
            }
            foreach (var recipe in _recipes[lang](context.ReferenceBook))
            {
                service.SaveRecipe(recipe);
            }
        }

        private static IEnumerable<Recipe> Russian(IReferenceBookService referenceBook)
        {
            return new List<Recipe>
            {
                new Recipe
                {
                    Title = "Обычная колбаса",
                    Category = referenceBook.LookupCategory(Constants.Categories.Appetizers),
                    CookingMinutes = 20,
                    CookingSteps = new List<string>
                    {
                        "Нарежьте колбасы",
                    },
                    Ingredients = new List<Ingredient>
                    {
                        new Ingredient {Measure = referenceBook.LookupMeasure(Constants.Measures.Cup), Quantity = 2, Title = "Водка"},
                        new Ingredient {Measure = referenceBook.LookupMeasure(Constants.Measures.Cup), Quantity = 2, Title = "Вода"}
                    }
                },
                new Recipe
                {
                    Title = "Сырнички",
                    Notes = "Моя очень важная записочка",
                    CookingMinutes = 150,
                    Category = referenceBook.LookupCategory(Constants.Categories.Appetizers),
                    CookingSteps = new List<string>
                    {
                        "Сделайте сырников"
                    }
                },

            };
        }

        private static IEnumerable<Recipe> English(IReferenceBookService referenceBook)
        {
            return new List<Recipe>
            {
            };
        } 
    }
}
