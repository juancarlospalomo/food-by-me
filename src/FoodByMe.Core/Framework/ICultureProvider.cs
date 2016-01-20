using System.Globalization;

namespace FoodByMe.Core.Framework
{
    public interface ICultureProvider
    {
        CultureInfo Culture { get; }
    }
}
