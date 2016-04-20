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
            var parameters = new List<object>();
            var filters = new List<string>();
            if (query.CategoryId != null)
            {
                filters.Add("(RecipeTextField.Type = 0 AND RecipeTextField.Value = ?)");
                parameters.Add(query.CategoryId.Value.ToString());
            }
            if (query.OnlyFavorite)
            {
                filters.Add("(RecipeTextField.Type = 1 AND RecipeTextField.Value LIKE '%Favorite%')");
            }
            if (!string.IsNullOrEmpty(query.SearchTerm))
            {
                parameters.Add(query.SearchTerm);
                filters.Add("RecipeTextSearch MATCH ?");
            }
            var filtersSql = filters.Count > 0
                ? $"WHERE {string.Join("AND", filters)}"
                : string.Empty;
            var sql = $@"SELECT Recipe.Id, Recipe.Document, 
                        MIN(CASE RecipeTextField.Type
                            WHEN 2 THEN 0
                            WHEN 0 THEN 1
                            WHEN 1 THEN 2
                            WHEN 3 THEN 4
                            WHEN 5 THEN 6
	                        ELSE 7
                         END) AS Priority
                        FROM RecipeTextField
                        JOIN Recipe ON Recipe.Id = RecipeTextField.RecipeId
                        JOIN RecipeTextSearch ON RecipeTextSearch.docid = RecipeTextField.Id
                        {filtersSql}    
                        GROUP BY Recipe.Id
                        ORDER BY Priority";
            var recipes = _connection.Query<RecipeRow>(sql, parameters.ToArray());
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
