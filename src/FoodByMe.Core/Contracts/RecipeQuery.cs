namespace FoodByMe.Core.Contracts
{
    public class RecipeQuery
    {
        public int? CategoryId { get; set; }

        public bool? IsFavorite { get; set; }

        public string SearchTerm { get; set; }
    }
}
