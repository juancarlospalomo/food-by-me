using SQLite.Net.Attributes;

namespace FoodByMe.Core.Services.Data.Types
{
    [Table("Recipe")]
    public class RecipeTable
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull]
        public RecipeBlob Recipe { get; set; }
    }
}
