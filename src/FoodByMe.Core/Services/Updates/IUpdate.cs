using System.Threading.Tasks;

namespace FoodByMe.Core.Services.Updates
{
    internal interface IUpdate
    {
        Task ApplyAsync();
    }
}
