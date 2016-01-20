using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Linq;
using FoodByMe.Core.Contracts;
using FoodByMe.Core.Contracts.Data;
using FoodByMe.Core.Services.Data;
using FoodByMe.Core.Services.Data.Indexing;
using FoodByMe.Core.Services.Data.Indexing.Stemmers;
using FoodByMe.Core.Services.Data.Serialization;
using FoodByMe.Core.Services.Data.Types;
using FoodByMe.Core.Services.Updates;
using MvvmCross.Platform.Platform;
using MvvmCross.Plugins.Sqlite;
using SQLite.Net.Async;

namespace FoodByMe.Core.Services
{
    public class DataService
    {
        private readonly IMvxSqliteConnectionFactory _connectionFactory;
        private readonly IMvxTrace _trace;
        private readonly Updater _updater;
        private readonly SQLiteAsyncConnection _connection;

        public DataService(IMvxSqliteConnectionFactory connectionFactory, IMvxTrace trace, DatabaseSettings settings)
        {
            if (connectionFactory == null)
            {
                throw new ArgumentNullException(nameof(connectionFactory));
            }
            if (trace == null)
            {
                throw new ArgumentNullException(nameof(trace));
            }
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }
            _connectionFactory = connectionFactory;
            _trace = trace;
            var config = new SqLiteConfig(settings.DatabaseName, serializer: new JsonBlobSerializer());
            _connection = connectionFactory.GetAsyncConnection(config);
            _updater = new Updater(typeof(DatabaseSchema).GetTypeInfo().Assembly, _connection, trace);
        }

        public Task UpdateDatabaseToLatestVersionAsync()
        {
            return _updater.UpdateToLatestVersionAsync();
        }

        public Task<Recipe> FindRecipeAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Recipe> SaveRecipeAsync(Recipe recipe)
        {
            if (recipe == null)
            {
                throw new ArgumentNullException(nameof(recipe));
            }
            //TODO: some validation logic

            if (recipe.Id == default(int))
            {
                await Insert(recipe).ConfigureAwait(false);
                return recipe;
            }
            await Update(recipe).ConfigureAwait(false);
            return recipe;
        }


        public Task<List<Recipe>> SearchRecipesAsync(RecipeQuery query)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<RecipeCategory>> ListCategoriesAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<List<Measure>> ListMeasuresAsync()
        {
            throw new System.NotImplementedException();
        }

        private async Task Insert(Recipe recipe)
        {
            var row = recipe.ToRecipeRow();
            await _connection.InsertAsync(row).ConfigureAwait(false);
            recipe.Id = row.Id;
            var fields = FieldExtractor.Extract(recipe);
            await _connection.InsertAllAsync(fields).ConfigureAwait(false);
            var culture = new CultureInfo("en-US");
            var stemmer = StemmerFactory.Create(culture);
            var searchFields = fields
                .Where(x => x.Type != RecipeTextType.CategoryId)
                .Select(x => ToSearchableField(x, stemmer))
                .ToList();
            await _connection.InsertAllAsync(searchFields).ConfigureAwait(false);
        }

        private RecipeTextSearchRow ToSearchableField(RecipeTextFieldRow field, IStemmer stemmer)
        {
            var row = new RecipeTextSearchRow
            {
                Id = field.Id,
                Text = string.Join(" ", Tokenize(field.Value).Select(stemmer.Stem))
            };
            return row;
        }

        private Task Update(Recipe recipe)
        {
            throw new NotImplementedException();
        }

        private static IEnumerable<string> Tokenize(string text)
        {
            var splitters = text.ToCharArray()
                .Where(x => !char.IsLetter(x))
                .ToArray();
            return text.Split(splitters, StringSplitOptions.RemoveEmptyEntries)
                .Where(x => x.Length > 2);
        } 
    }
}
