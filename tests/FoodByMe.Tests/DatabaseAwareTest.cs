using System.IO;
using System.Threading.Tasks;
using FoodByMe.Core.Services.Data;
using FoodByMe.Core.Services.Data.Serialization;
using FoodByMe.Core.Services.Updates;
using FoodByMe.Tests.Utilities;
using MvvmCross.Plugins.Sqlite;
using MvvmCross.Plugins.Sqlite.Wpf;
using NUnit.Framework;
using SQLite.Net;
using SQLite.Net.Async;

namespace FoodByMe.Tests
{
    [TestFixture]
    public abstract class DatabaseAwareTest
    {
        private const string DatabaseName = "FoodByMe.Test.db3";

        protected readonly WindowsSqliteConnectionFactory ConnectionFactory = new WindowsSqliteConnectionFactory();

        protected SQLiteConnection Connection { get; private set; }

        protected DatabaseSettings DatabaseSettings => new DatabaseSettings(DatabaseName);

        [SetUp]
        public void RecreateDatabase()
        {
            Connection?.Close();
            var name = ConnectionFactory.GetPlattformDatabasePath(DatabaseName);
            File.Delete(name);
            using (var updater = new UpdateService(ConnectionFactory, new ConsoleTrace(), DatabaseSettings))
            {
                updater.UpdateToLatestVersion();
            }
            var config = new SqLiteConfig(DatabaseName, serializer: new JsonBlobSerializer());
            Connection = ConnectionFactory.GetConnection(config);
        }
    }
}
