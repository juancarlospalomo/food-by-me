using SQLite.Net.Attributes;

namespace FoodByMe.Core.Services.Data.Types
{
    [Table("RecipeTextField")]
    internal class RecipeTextFieldRow
    {
        public RecipeTextFieldRow()
        {
            IsSearchable = true;
        }

        [PrimaryKey, AutoIncrement]
        public long Id { get; set; }

        [NotNull]
        public int RecipeId { get; set; }

        [Ignore]
        public bool IsSearchable { get; set; }

        [NotNull]
        [Indexed]
        public RecipeTextType Type { get; set; }

        [Indexed]
        [NotNull]
        public string Value { get; set; }
    }
}
