using System.Globalization;
using FoodByMe.Core.Services.Data;
using FoodByMe.Core.Services.Data.Types;
using FoodByMe.Tests.Utilities;
using NUnit.Framework;

namespace FoodByMe.Tests
{
    [TestFixture]
    public class SearchServiceTest : DatabaseAwareTest
    {
        [Test]
        public void SearchRecipes_WithNullQuery()
        {
            Connection.Insert(new RecipeRow
            {
                Id = 1,
                Recipe = new RecipeBlob {Title = "Recipe #1"},
            });
            Connection.Insert(new RecipeRow
            {
                Id = 2,
                Recipe = new RecipeBlob { Title = "Recipe #2" }
            });

            using (var service = Create())
            {
                var list = service.SearchRecipes(null);
                Assert.AreEqual(2, list.Count);
            }
        }

        private SearchService Create(CultureInfo culture = null)
        {
            var provider = new TestCultureProvider(culture ?? new CultureInfo("en-US"));
            var service = new SearchService(ConnectionFactory, provider, DatabaseSettings);
            return service;
        } 
    }
}
