using FoodByMe.Core.Contracts.Data;
using FoodByMe.Core.Resources;

namespace FoodByMe.Core.ViewModels
{
    public class IngredientDisplayViewModel : BaseViewModel
    {
        public int Position { get; set; }

        public string Title { get; set; }

        public double? Quantity { get; set; }

        public Measure Measure { get; set; }

        public string QuantityText => Quantity.HasValue ? $"{Quantity} {Measure.ShortTitle}" : Text.ReferenceMeasureTaste;
    }
}