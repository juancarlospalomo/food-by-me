using System.Threading.Tasks;

namespace FoodByMe.Core.Contracts
{
    public interface IUpdateService
    {
        Task UpdateToLatestVersionAsync();
    }
}
