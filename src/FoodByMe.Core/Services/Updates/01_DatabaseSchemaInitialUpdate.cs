using FoodByMe.Core.Services.Data;
using FoodByMe.Core.Services.Data.Types;

namespace FoodByMe.Core.Services.Updates
{
    [Update(1, "Initial database schema")]
    internal class DatabaseSchemaInitialUpdate : IUpdate
    {

        public void Apply(UpdateContext context)
        {
            var connection = context.Connection;
            connection.CreateTable<RecipeRow>();
            connection.CreateTable<RecipeTextFieldRow>();
            connection.Execute("CREATE VIRTUAL TABLE RecipeTextSearch USING fts4(content='RecipeTextField', Value)");
            connection.Execute(@"CREATE TRIGGER FtsTriggerUpdate BEFORE UPDATE ON RecipeTextField BEGIN
                                     DELETE FROM RecipeTextSearch WHERE docid=old.rowid;
                                 END;");
            connection.Execute(@"CREATE TRIGGER FtsTriggerDelete BEFORE DELETE ON RecipeTextField BEGIN
                                      DELETE FROM RecipeTextSearch WHERE docid=old.rowid;
                                  END;");

        }
    }
}
