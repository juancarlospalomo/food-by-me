using System.Threading.Tasks;

namespace FoodByMe.Core.Services.Data
{
    internal interface IUpdate
    {
        Task ApplyAsync();
    }
}
