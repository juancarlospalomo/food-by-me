using System;
using SQLite.Net.Attributes;

namespace FoodByMe.Core.Services.Data.Types
{
    [Table("Version")]
    public class VersionRow
    {
        [PrimaryKey]
        public int Version { get; set; }

        [NotNull]
        public DateTime Timestamp { get; set; }

        [NotNull]
        public string Tag { get; set; }
    }
}
