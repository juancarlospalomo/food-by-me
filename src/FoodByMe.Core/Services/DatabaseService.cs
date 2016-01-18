using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using FoodByMe.Core.Contracts;
using FoodByMe.Core.Services.Data;
using FoodByMe.Core.Services.Data.Types;
using FoodByMe.Core.Services.Data.Updates;
using MvvmCross.Platform.Platform;
using MvvmCross.Plugins.Sqlite;
using SQLite.Net.Async;

namespace FoodByMe.Core.Services
{
    public class DatabaseService : IDatabaseService
    {
        private readonly IMvxSqliteConnectionFactory _connectionFactory;
        private readonly Updater _updater;
        private readonly SQLiteAsyncConnection _connection;

        public DatabaseService(IMvxSqliteConnectionFactory connectionFactory, IMvxTrace trace, DatabaseSettings settings)
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
            var config = new SqLiteConfig(settings.DatabaseName, serializer: new JsonBlobSerializer());
            _connection = connectionFactory.GetAsyncConnection(config);
            _updater = new Updater(typeof(InitialSchema).GetTypeInfo().Assembly, _connection, trace);
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
                var id = await Insert(recipe).ConfigureAwait(false);
                recipe.Id = id;
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

        private async Task<int> Insert(Recipe recipe)
        {
            var row = recipe.ToRecipeTable();
            var id = await _connection.InsertAsync(row).ConfigureAwait(false);
            recipe.Id = id;
            var fields = RecipeIndexer.CreateIndices(recipe);
            await _connection.InsertAllAsync(fields).ConfigureAwait(false);
            return id;
        }

        private Task Update(Recipe recipe)
        {
            throw new NotImplementedException();
        }
    }
}
