namespace FoodByMe.Core.Contracts.Data
{
    public class RecipeQuery
    {
        public int? CategoryId { get; set; }

        public bool OnlyFavorite { get; set; }

        public string SearchTerm { get; set; }

        public bool IsSearch => !string.IsNullOrWhiteSpace(SearchTerm);
    }
}
