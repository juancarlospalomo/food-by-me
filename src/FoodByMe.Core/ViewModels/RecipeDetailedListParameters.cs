namespace FoodByMe.Core.ViewModels
{
    public class RecipeDetailedListParameters
    {
        public int RecipeId { get; set; }

        public int? CategoryId { get; set; }

        public bool? IsFavorite { get; set; }

        public string SearchTerm { get; set; }
    }
}
