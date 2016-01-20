using SQLite.Net.Attributes;

namespace FoodByMe.Core.Services.Data.Types
{
    [Table("Recipe")]
    public class RecipeRow
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull]
        [Column("Document")]
        public RecipeBlob Recipe { get; set; }
    }
}
