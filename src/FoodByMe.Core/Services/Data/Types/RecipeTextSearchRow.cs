using SQLite.Net.Attributes;

namespace FoodByMe.Core.Services.Data.Types
{
    [Table("RecipeTextSearch")]
    public class RecipeTextSearchRow
    {
        [Column("docid")]
        public long Id { get; set; }

        [NotNull]
        public string Text { get; set; }
    }
}
