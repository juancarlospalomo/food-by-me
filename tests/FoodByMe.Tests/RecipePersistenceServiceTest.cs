using System.Collections.Generic;
using System.Globalization;
using FoodByMe.Core.Contracts.Data;
using FoodByMe.Core.Services.Data;
using FoodByMe.Tests.Utilities;
using NUnit.Framework;

namespace FoodByMe.Tests
{
    [TestFixture]
    public class RecipePersistenceServiceTest : DatabaseAwareTest
    {
        private SearchService _search;

        [Test]
        public void Create_And_Find_Recipe()
        {
            using (var service = Create())
            {
                var recipe = new Recipe
                {
                    Title = "Cheese",
                    Category = new RecipeCategory
                    {
                        Id = 1,
                        Title = "Desserts"
                    },
                    CookingMinutes = 30,
                    Ingredients = new List<Ingredient>
                {
                    new Ingredient
                    {
                        Measure = new Measure {Id = 1, Title = "pieces"},
                        Quantity = 2,
                        Title = "Apple"
                    }
                },
                    CookingSteps = new List<string>
                {
                    "Prepare your kitchen",
                    "Do something else"
                }
                };
                recipe = service.SaveRecipe(recipe);
                var persisted = service.FindRecipe(recipe.Id);
                Assert.IsNotNull(persisted);
            }

        }

        [Test]
        public void Create_Update_And_Find_Recipe()
        {
            using (var service = Create())
            {
                var recipe = new Recipe
                {
                    Title = "Cheese",
                    Category = new RecipeCategory
                    {
                        Id = 1,
                        Title = "Desserts"
                    },
                    CookingMinutes = 30,
                    Ingredients = new List<Ingredient>
                {
                    new Ingredient
                    {
                        Measure = new Measure {Id = 1, Title = "pieces"},
                        Quantity = 2,
                        Title = "Apple"
                    }
                },
                    CookingSteps = new List<string>
                {
                    "Prepare your kitchen",
                    "Do something else"
                }
                };
                service.SaveRecipe(recipe);
                recipe.Title = "Apple pie";
                recipe.CookingSteps.Add("Do something else one more time");
                service.SaveRecipe(recipe);
                recipe = service.FindRecipe(recipe.Id);

                Assert.AreEqual(recipe.Title, "Apple pie");
                Assert.AreEqual(3, recipe.CookingSteps.Count);
            }
        }

        [TearDown]
        public void DisposeDependencies()
        {    
            _search.Dispose();
        }

        private RecipePersistenceService Create(CultureInfo culture = null)
        {
            culture = culture ?? CultureInfo.CurrentUICulture;
            var provider = new TestCultureProvider(culture);
            _search = new SearchService(ConnectionFactory, provider, DatabaseSettings);
            var service = new RecipePersistenceService(ConnectionFactory, provider, _search, DatabaseSettings);
            return service;
        }
    }
}
