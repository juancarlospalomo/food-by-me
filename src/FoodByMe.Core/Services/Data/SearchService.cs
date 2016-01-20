using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodByMe.Core.Contracts;
using FoodByMe.Core.Contracts.Data;
using FoodByMe.Core.Framework;
using FoodByMe.Core.Services.Data.Indexing;
using FoodByMe.Core.Services.Data.Indexing.Stemmers;
using FoodByMe.Core.Services.Data.Serialization;
using FoodByMe.Core.Services.Data.Types;
using MvvmCross.Plugins.Sqlite;
using Nito.AsyncEx;
using SQLite.Net.Async;

namespace FoodByMe.Core.Services.Data
{
    public class SearchService : ISearchService, IReferenceBookService
    {
        private readonly ICultureProvider _cultureProvider;
        private readonly AsyncLock _lock = new AsyncLock();
        private readonly SQLiteAsyncConnection _connection;
        private readonly QueryParser _parser;

        public SearchService(IMvxSqliteConnectionFactory connectionFactory, ICultureProvider cultureProvider, DatabaseSettings settings)
        {
            _cultureProvider = cultureProvider;
            if (connectionFactory == null)
            {
                throw new ArgumentNullException(nameof(connectionFactory));
            }
            if (cultureProvider == null)
            {
                throw new ArgumentNullException(nameof(cultureProvider));
            }
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }
            _parser = new QueryParser(cultureProvider);
            var config = new SqLiteConfig(settings.DatabaseName, serializer: new JsonBlobSerializer());
            _connection = connectionFactory.GetAsyncConnection(config);
        }

        public async Task<Recipe> FindRecipeAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }
            RecipeRow row = null;
            using (await _lock.LockAsync())
            {
                row = await _connection.GetAsync<RecipeRow>(x => x.Id == id).ConfigureAwait(false);
            }
            return row?.ToRecipe(this);
        }

        public Task<List<Recipe>> SearchRecipesAsync(RecipeQuery query)
        {
            throw new NotImplementedException();
        }

        public async Task<List<string>> SearchIngredients(string query)
        {
            var q = _parser.Parse(query);
            if (q == null)
            {
                return new List<string>();
            }
            var list = await _connection.Table<RecipeTextFieldRow>()
                .Where(x => x.Type == RecipeTextType.Ingredient && x.Value.Contains(q))
                .ToListAsync()
                .ConfigureAwait(false);
            return list.Select(x => x.Value).ToList();
        }

        public RecipeCategory LookupCategory(int id)
        {
            if (id < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }
            var categories = StaticData.Categories(_cultureProvider.Culture);
            return categories.ContainsKey(id) ? categories[id] : null;
        }

        public Measure LookupMeasure(int id)
        {
            if (id < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }
            var measures = StaticData.Measures(_cultureProvider.Culture);
            return measures.ContainsKey(id) ? measures[id] : null;
        }

        public Task<List<RecipeCategory>> ListCategoriesAsync()
        {
            return Task.FromResult(StaticData.Categories(_cultureProvider.Culture).Values.ToList());
        }

        public Task<List<Measure>> ListMeasuresAsync()
        {
            return Task.FromResult(StaticData.Measures(_cultureProvider.Culture).Values.ToList());
        }
    }
}
