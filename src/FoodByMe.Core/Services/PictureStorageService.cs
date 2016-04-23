using System;
using System.Threading.Tasks;
using FoodByMe.Core.Contracts;
using PCLStorage;
namespace FoodByMe.Core.Services
{
    public class PictureStorageService : IPictureStorageService
    {
        private readonly IPictureOptimizer _optimizer;
        private IFolder _folder;

        public PictureStorageService(IPictureOptimizer optimizer)
        {
            if (optimizer == null)
            {
                throw new ArgumentNullException(nameof(optimizer));
            }
            _optimizer = optimizer;
        }

        public async Task DeleteAsync(string path)
        {
            var file = await FileSystem.Current.GetFileFromPathAsync(path).ConfigureAwait(false);
            if (file != null)
            {
                await file.DeleteAsync().ConfigureAwait(false);
            }
        }

        public async Task<string> SaveAsync(string path)
        {
            var outputFolder = await EnsureFolderExists().ConfigureAwait(false);
            var outputFile = await outputFolder.CreateFileAsync(GenerateUniqueName(), CreationCollisionOption.OpenIfExists);
            var inputFile = await FileSystem.Current.GetFileFromPathAsync(path).ConfigureAwait(false);
            using (var input = await inputFile.OpenAsync(FileAccess.Read).ConfigureAwait(false))
            using (var output = await outputFile.OpenAsync(FileAccess.ReadAndWrite).ConfigureAwait(false))
            {
                await _optimizer.OptimizeAsync(input, output).ConfigureAwait(false);
            }
            return outputFile.Path;
        }

        private async Task<IFolder> EnsureFolderExists()
        {
            if (_folder != null)
            {
                return _folder;
            }
            _folder = await FileSystem.Current.LocalStorage
                .CreateFolderAsync("Pictures", CreationCollisionOption.OpenIfExists)
                .ConfigureAwait(false);
            return _folder;
        }

        private string GenerateUniqueName()
        {
            return $"foodbyme-{Guid.NewGuid()}.jpg";
        }
    }
}
