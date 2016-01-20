using System;
using System.Threading.Tasks;
using FoodByMe.Core.Services.Data;
using FoodByMe.Core.Services.Data.Types;
using SQLite.Net.Async;

namespace FoodByMe.Core.Services.Updates
{
    [Update(1, "Initial database schema")]
    internal class DatabaseSchema : IUpdate
    {
        private readonly SQLiteAsyncConnection _connection;

        public DatabaseSchema(SQLiteAsyncConnection connection)
        {
            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }
            _connection = connection;
        }

        public async Task ApplyAsync()
        {
            await _connection.CreateTablesAsync<RecipeRow, RecipeTextFieldRow>().ConfigureAwait(false);
            await _connection.ExecuteAsync("CREATE VIRTUAL TABLE RecipeTextSearch USING fts4(content='RecipeTextField', Text)");
        }
    }
}
