using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FoodByMe.Core.Services.Data;
using FoodByMe.Core.Services.Data.Types;
using MvvmCross.Platform.Platform;
using Nito.AsyncEx;
using SQLite.Net.Async;

namespace FoodByMe.Core.Services.Updates
{
    internal class Updater
    {
        private readonly Assembly _assembly;
        private readonly SQLiteAsyncConnection _connection;
        private readonly IMvxTrace _trace;
        private readonly AsyncLock _lock = new AsyncLock();

        public Updater(Assembly assembly, SQLiteAsyncConnection connection, IMvxTrace trace)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }
            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }
            if (trace == null)
            {
                throw new ArgumentNullException(nameof(trace));
            }
            _trace = trace;
            _assembly = assembly;
            _connection = connection;
        }

        public async Task UpdateToLatestVersionAsync()
        {
            using (await _lock.LockAsync())
            {
                var version = await GetCurrentVersion().ConfigureAwait(false);
                var updates = GetPendingUpdates(version);
                foreach (var update in updates)
                {
                    var meta = update.Item2;
                    var code = update.Item1;
                    Trace($"Applying updated #{meta.Version} [{meta.Tag}]...");
                    await code.ApplyAsync().ConfigureAwait(false);
                    var row = new VersionRow { Tag = meta.Tag, Timestamp = new DateTime(), Version = meta.Version };
                    await _connection.InsertAsync(row).ConfigureAwait(false);
                    Trace($"Update #{meta.Version} [{meta.Tag}] was successfully applied.");
                }
            }
        }

        private async Task<int> GetCurrentVersion()
        {
            await _connection.CreateTableAsync<VersionRow>();
            var version = await _connection.Table<VersionRow>()
                .OrderByDescending(x => x.Version)
                .FirstOrDefaultAsync();
            return version?.Version ?? 0;
        }

        private IEnumerable<Tuple<IUpdate, UpdateAttribute>> GetPendingUpdates(int baseVersion)
        {
            var types = _assembly.DefinedTypes
                .Where(t => typeof (IUpdate).GetTypeInfo().IsAssignableFrom(t) && t.IsClass && !t.IsAbstract)
                .Select(t => new
                {
                    Type = t,
                    Meta = t.GetCustomAttribute<UpdateAttribute>()
                })
                .ToList();

            if (types.Any(x => x.Meta == null))
            {
                throw new InvalidOperationException("Not all update classes are annotated with UpdateAttribute.");
            }

            var updates = types
                .OrderBy(x => x.Meta.Version)
                .Where(x => x.Meta.Version > baseVersion)
                .Select(x => Tuple.Create((IUpdate)Activator.CreateInstance(x.Type.AsType(), _connection), x.Meta));
            return updates;
        }

        private void Trace(string message)
        {
            const string tag = "Updater";
            _trace.Trace(MvxTraceLevel.Diagnostic, tag, message);
        }
    }
}