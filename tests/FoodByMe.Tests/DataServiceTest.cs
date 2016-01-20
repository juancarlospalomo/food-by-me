using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FoodByMe.Core.Contracts;
using FoodByMe.Core.Contracts.Data;
using FoodByMe.Core.Services;
using FoodByMe.Tests.Utilities;
using MvvmCross.Plugins.Sqlite.Wpf;
using NUnit.Framework;

namespace FoodByMe.Tests
{
    [TestFixture]
    public class DataServiceTest
    {
        private const string DatabaseName = "FoodByMe.Test.db3";

        private readonly WindowsSqliteConnectionFactory _factory = new WindowsSqliteConnectionFactory();

        private readonly ConsoleTrace _trace = new ConsoleTrace();

        [Test]
        public async Task UpdateDatabaseToLatestVersion_RunsWithoutExceptions()
        {
            var service = await Create(false);
            await service.UpdateDatabaseToLatestVersionAsync();
        }

        [Test]
        public async Task SaveRecipe_ReturnsRecipeWithId()
        {
            var service = await Create();
            var recipe = new Recipe
            {
                Title = "Cheese",
                Category = new RecipeCategory
                {
                    Id = 3,
                    Title = "Desserts"
                },
                Ingredients = new List<Ingredient>
                {
                    new Ingredient
                    {
                        Measure = new Measure {Id = 1, Title = "items"},
                        Quantity = 2,
                        Title = "Apple"
                    }
                }
            };
            recipe = await service.SaveRecipeAsync(recipe);
            Assert.AreNotEqual(0, recipe.Id);
        }

        [Test]
        public async Task SearchRecipe_ReturnsSortedRecipes()
        {
            var service = await Create();
            var recipe1 = new Recipe
            {
                Title = "Cheese",
                Category = new RecipeCategory
                {
                    Id = 3,
                    Title = "Desserts"
                },
                Ingredients = new List<Ingredient>
                {
                    new Ingredient
                    {
                        Measure = new Measure {Id = 1, Title = "items"},
                        Quantity = 2,
                        Title = "Apple"
                    }
                }
            };
            await service.SaveRecipeAsync(recipe1);
            var recipe2 = new Recipe
            {
                Title = "Apple pie",
                Category = new RecipeCategory
                {
                    Id = 3,
                    Title = "Desserts"
                },
                Ingredients = new List<Ingredient>
                {
                    new Ingredient
                    {
                        Measure = new Measure {Id = 1, Title = "liter"},
                        Quantity = 1,
                        Title = "Milk"
                    },
                    new Ingredient
                    {
                        Measure = new Measure {Id = 1, Title = "items"},
                        Quantity = 5,
                        Title = "Polish apples"
                    }
                }
            };

            await service.SaveRecipeAsync(recipe2);
        }

        private async Task<DataService> Create(bool migrate = true)
        {
            var name = _factory.GetPlattformDatabasePath(DatabaseName);
            File.Delete(name);
            var settings = new DatabaseSettings("FoodByMe.Test.db3");
            var service = new DataService(_factory, _trace, settings);
            if (migrate)
            {
                await service.UpdateDatabaseToLatestVersionAsync();
            }
            return service;
        }
    }
}
