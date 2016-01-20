using System;

namespace FoodByMe.Core.Services.Data
{
    public class DatabaseSettings
    {
        public DatabaseSettings(string databaseName)
        {
            if (string.IsNullOrEmpty(databaseName))
            {
                throw new ArgumentNullException(nameof(databaseName));
            }
            DatabaseName = databaseName;
        }

        public string DatabaseName { get; }
    }
}
