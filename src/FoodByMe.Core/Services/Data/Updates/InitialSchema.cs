using System;
using System.Threading.Tasks;
using FoodByMe.Core.Services.Data.Types;
using SQLite.Net.Async;

namespace FoodByMe.Core.Services.Data.Updates
{
    [Update(1, "Initial database schema")]
    internal class InitialSchema : IUpdate
    {
        private readonly SQLiteAsyncConnection _connection;

        public InitialSchema(SQLiteAsyncConnection connection)
        {
            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }
            _connection = connection;
        }

        public async Task ApplyAsync()
        {
            await _connection.CreateTablesAsync<RecipeTable, RecipeTextFieldTable>().ConfigureAwait(false);
            await _connection.ExecuteAsync("CREATE VIRTUAL TABLE RecipeTextFieldFts USING fts4(content='RecipeTextField', Value)");
        }
    }
}
