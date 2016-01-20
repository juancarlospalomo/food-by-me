using System;
using System.Collections.Generic;
using System.Linq;
using FoodByMe.Core.Contracts;
using FoodByMe.Core.Contracts.Data;
using FoodByMe.Core.Framework;
using FoodByMe.Core.Services.Data.Indexing;
using FoodByMe.Core.Services.Data.Serialization;
using FoodByMe.Core.Services.Data.Types;
using MvvmCross.Plugins.Sqlite;
using SQLite.Net;

namespace FoodByMe.Core.Services.Data
{
    public class SearchService : ISearchService, IDisposable
    {
        private readonly ICultureProvider _cultureProvider;
        private SQLiteConnectionWithLock _connection;
        private readonly QueryParser _parser;
        private bool _isDisposed;

        public SearchService(IMvxSqliteConnectionFactory connectionFactory, ICultureProvider cultureProvider, DatabaseSettings settings)
        {
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
            _cultureProvider = cultureProvider;
            _parser = new QueryParser(cultureProvider);
            var config = new SqLiteConfig(settings.DatabaseName, serializer: new JsonBlobSerializer());
            _connection = connectionFactory.GetConnectionWithLock(config);
        }

        public List<Recipe> SearchRecipes(RecipeQuery query)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(nameof(SearchService));
            }
            //TODO filtering and sorting
            query = query ?? new RecipeQuery();
            var recipes = _connection.Table<RecipeRow>()
                .ToList();
            return recipes.Select(x => x.ToRecipe(this)).ToList();
        }

        public List<string> SearchIngredients(string query)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(nameof(SearchService));
            }
            var q = _parser.Parse(query);
            if (q == null)
            {
                return new List<string>();
            }
            List<RecipeTextFieldRow> list;
            using (_connection.Lock())
            {
                list = _connection.Table<RecipeTextFieldRow>()
                    .Where(x => x.Type == RecipeTextType.Ingredient && x.Value.Contains(q))
                    .ToList();
            }
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

        public List<RecipeCategory> ListCategories()
        {
            return StaticData.Categories(_cultureProvider.Culture).Values.ToList();
        }

        public List<Measure> ListMeasures()
        {
            return StaticData.Measures(_cultureProvider.Culture).Values.ToList();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (_isDisposed || !disposing)
            {
                return;
            }
            _isDisposed = true;
            _connection?.Close();
            _connection = null;
        }
    }
}
