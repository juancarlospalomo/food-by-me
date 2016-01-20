using System;
using System.Reflection;
using System.Threading.Tasks;
using FoodByMe.Core.Contracts;
using FoodByMe.Core.Services.Data;
using FoodByMe.Core.Services.Data.Serialization;
using MvvmCross.Platform.Platform;
using MvvmCross.Plugins.Sqlite;

namespace FoodByMe.Core.Services.Updates
{
    public class UpdateService : IUpdateService
    {
        private readonly Updater _updater;

        public UpdateService(IMvxSqliteConnectionFactory factory, IMvxTrace trace, DatabaseSettings settings)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }
            if (trace == null)
            {
                throw new ArgumentNullException(nameof(trace));
            }
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }
            var config = new SqLiteConfig(settings.DatabaseName, serializer: new JsonBlobSerializer());
            var connection = factory.GetAsyncConnection(config);
            _updater = new Updater(typeof(DatabaseSchema).GetTypeInfo().Assembly, connection, trace);
        }

        public Task UpdateToLatestVersionAsync()
        {
            return _updater.UpdateToLatestVersionAsync();
        }
    }
}
