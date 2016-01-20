using System;
using System.Reflection;
using FoodByMe.Core.Contracts;
using FoodByMe.Core.Services.Data;
using FoodByMe.Core.Services.Data.Serialization;
using MvvmCross.Platform.Platform;
using MvvmCross.Plugins.Sqlite;
using SQLite.Net;

namespace FoodByMe.Core.Services.Updates
{
    public class UpdateService : IUpdateService, IDisposable
    {
        private readonly Updater _updater;
        private bool _isDisposed;
        private SQLiteConnectionWithLock _connection;

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
            _connection = factory.GetConnectionWithLock(config);
            _updater = new Updater(typeof(DatabaseSchemaInitialUpdate).GetTypeInfo().Assembly, _connection, trace);
        }

        public void UpdateToLatestVersion()
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(nameof(UpdateService));
            }
            _updater.UpdateToLatestVersion();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _connection?.Close();
                _connection = null;
            }
            _isDisposed = true;
        }
    }
}
