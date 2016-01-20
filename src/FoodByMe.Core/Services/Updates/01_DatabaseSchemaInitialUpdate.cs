using System;
using FoodByMe.Core.Services.Data;
using FoodByMe.Core.Services.Data.Types;
using SQLite.Net;

namespace FoodByMe.Core.Services.Updates
{
    [Update(1, "Initial database schema")]
    internal class DatabaseSchemaInitialUpdate : IUpdate
    {
        private readonly SQLiteConnection _connection;

        public DatabaseSchemaInitialUpdate(SQLiteConnection connection)
        {
            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }
            _connection = connection;
        }

        public void Apply()
        {
            _connection.CreateTable<RecipeRow>();
            _connection.CreateTable<RecipeTextFieldRow>();
            _connection.Execute("CREATE VIRTUAL TABLE RecipeTextSearch USING fts4(content='RecipeTextField', Value)");
            _connection.Execute(@"CREATE TRIGGER FtsTriggerUpdate BEFORE UPDATE ON RecipeTextField BEGIN
                                      DELETE FROM RecipeTextSearch WHERE docid=old.rowid;
                                  END;");
            _connection.Execute(@"CREATE TRIGGER FtsTriggerDelete BEFORE DELETE ON RecipeTextField BEGIN
                                      DELETE FROM RecipeTextSearch WHERE docid=old.rowid;
                                  END;");

        }
    }
}
