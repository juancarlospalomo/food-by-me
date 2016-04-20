using System.IO;
using System.Threading.Tasks;

namespace FoodByMe.Core.Contracts
{
    public interface IPictureOptimizer
    {
        Task OptimizeAsync(Stream input, Stream output);
    }
}
