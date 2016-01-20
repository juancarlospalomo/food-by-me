using SQLite.Net.Attributes;

namespace FoodByMe.Core.Services.Data.Types
{
    [Table("RecipeTextField")]
    internal class RecipeTextFieldRow
    {
        [PrimaryKey, AutoIncrement]
        public long Id { get; set; }

        [NotNull]
        public int RecipeId { get; set; }

        [NotNull]
        [Indexed]
        public RecipeTextType Type { get; set; }

        [Indexed]
        [NotNull]
        public string Value { get; set; }
    }
}
