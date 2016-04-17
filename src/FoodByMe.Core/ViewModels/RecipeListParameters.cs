namespace FoodByMe.Core.ViewModels
{
    public class RecipeListParameters
    {
        public int CategoryId { get; set; }

        public bool CategorySelected { get; set; }

        public bool IsFavoriteSelected { get; set; }

        public string SearchTerm { get; set; }
    }
}