using System.Threading.Tasks;

namespace FoodByMe.Core.Contracts
{
    public interface IPictureStorageService
    {
        Task DeleteAsync(string path);

        Task<string> SaveAsync(string path);
    }
}
