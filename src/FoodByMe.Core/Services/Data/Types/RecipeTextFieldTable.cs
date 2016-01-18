using System;
using SQLite.Net.Attributes;

namespace FoodByMe.Core.Services.Data.Types
{
    [Table("RecipeTextField")]
    internal class RecipeTextFieldTable
    {
        public RecipeTextFieldTable()
        {
            Id = Guid.NewGuid();
        }

        [PrimaryKey]
        public Guid Id { get; private set; }

        [NotNull]
        public int RecipeId { get; set; }

        [NotNull]
        [Indexed]
        public RecipeFieldType Type { get; set; }

        [Indexed]
        [NotNull]
        public string Value { get; set; }
    }
}
