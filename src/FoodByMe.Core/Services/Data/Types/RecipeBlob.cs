using System.Collections.Generic;

namespace FoodByMe.Core.Services.Data.Types
{
    public class RecipeBlob
    {
        public string Title { get; set; }

        public bool IsFavorite { get; set; }

        public string ImageUri { get; set; }

        public int CookingMinutes { get; set; }

        public int CategoryId { get; set; }

        public List<string> CookingSteps { get; set; }

        public List<IngredientBlob> Ingredients { get; set; }

        public string Notes { get; set; }
    }
}