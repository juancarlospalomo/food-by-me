using System.Collections.Generic;

namespace FoodByMe.Core.Contracts.Data
{
    public class Recipe
    {
        public Recipe()
        {
            CookingSteps = new List<string>();
            Ingredients = new List<Ingredient>();
        }

        public int Id { get; internal set; }

        public string Title { get; set; }

        public string ImageUri { get; set; }

        public bool IsFavorite { get; set; }

        public int CookingMinutes { get; set; }

        public RecipeCategory Category { get; set; }

        public List<string> CookingSteps { get; set; }

        public List<Ingredient> Ingredients { get; set; }

        public string Notes { get; set; }
    }
}
