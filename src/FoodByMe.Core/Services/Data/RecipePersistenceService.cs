using System;
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
    public class RecipePersistenceService : IRecipePersistenceService, IDisposable
    {
        private readonly IReferenceBookService _referenceBook;
        private SQLiteConnectionWithLock _connection;
        private readonly QueryParser _queryParser;
        private bool _isDisposed;

        public RecipePersistenceService(IMvxSqliteConnectionFactory connectionFactory,
            ICultureProvider cultureProvider,
            IReferenceBookService referenceBook,
            DatabaseSettings settings)
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
            if (referenceBook == null)
            {
                throw new ArgumentNullException(nameof(referenceBook));
            }
            _referenceBook = referenceBook;
            var config = new SqLiteConfig(settings.DatabaseName, serializer: new JsonBlobSerializer());
            _connection = connectionFactory.GetConnectionWithLock(config);
            _queryParser = new QueryParser(cultureProvider);
        }

        public Recipe SaveRecipe(Recipe recipe)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(nameof(RecipePersistenceService));
            }
            if (recipe == null)
            {
                throw new ArgumentNullException(nameof(recipe));
            }
            return recipe.Id == default(int)
                ? CreateRecipe(recipe)
                : UpdateRecipe(recipe);
        }

        public Recipe FindRecipe(int id)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(nameof(RecipePersistenceService));
            }
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }
            RecipeRow row = null;
            using (_connection.Lock())
            {
                row = _connection.Get<RecipeRow>(x => x.Id == id);
            }
            return row?.ToRecipe(_referenceBook);
        }

        private Recipe CreateRecipe(Recipe recipe)
        {
            var row = recipe.ToRecipeRow();

            using (_connection.Lock())
            {
                _connection.RunInTransaction(() =>
                {
                    _connection.Insert(row);
                    recipe.Id = row.Id;
                    var fields = FieldExtractor.Extract(recipe);
                    _connection.InsertAll(fields);
                    var searchFields = fields
                        .Where(x => x.IsSearchable)
                        .Select(x => new RecipeTextSearchRow
                        {
                            Id = x.Id,
                            Value = _queryParser.Parse(x.Value)
                        })
                        .ToList();
                    _connection.InsertAll(searchFields);
                });
            }
            return recipe;
        }

        private Recipe UpdateRecipe(Recipe recipe)
        {
            var row = recipe.ToRecipeRow();
            var fields = FieldExtractor.Extract(recipe);
            var searchFields = fields
                .Where(x => x.IsSearchable)
                .Select(x => new RecipeTextSearchRow
                {
                    Id = x.Id,
                    Value = _queryParser.Parse(x.Value)
                })
                .ToList();

            using (_connection.Lock())
            {
                //Remove all fields
                var mapping = _connection.GetMapping<RecipeTextFieldRow>();
                _connection.Execute($"DELETE FROM {mapping.TableName} WHERE RecipeId = ?", recipe.Id);

                //Recreate recipe and indices
                _connection.Update(row);
                _connection.InsertAll(fields);
                _connection.InsertAll(searchFields);
            }
            return recipe;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing || _isDisposed)
            {
                return;
            }
            _connection?.Close();
            _connection = null;
            _isDisposed = true;
        }
    }
}