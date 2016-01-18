using System;
using System.Threading.Tasks;
using FoodByMe.Core.Contracts;
using FoodByMe.Core.Services;
using FoodByMe.Tests.Utilities;
using MvvmCross.Plugins.Sqlite.Wpf;
using NUnit.Framework;

namespace FoodByMe.Tests
{
    [TestFixture]
    public class DatabaseServiceTest
    {
        private readonly WindowsSqliteConnectionFactory _factory = new WindowsSqliteConnectionFactory();

        [Test]
        public async Task UpdateDatabaseToLatestVersion_RunsWithoutExceptions()
        {
            var service = Create();
            await service.UpdateDatabaseToLatestVersionAsync();
            Console.WriteLine(Environment.CurrentDirectory);
        }

        [Test]
        public async Task SaveRecipe_ReturnsRecipeWithId()
        {
            var service = Create();
            var recipe = new Recipe {Title = "Some Title", Category = new RecipeCategory {Id = 3, Title = "Something"}};
            await service.SaveRecipeAsync(recipe);
        }

        private static DatabaseService Create()
        {
            var factory = new WindowsSqliteConnectionFactory();
            var trace = new ConsoleTrace();
            var settings = new DatabaseSettings("FoodByMe.Test.db3");
            return new DatabaseService(factory, trace, settings);
        }
    }


}
