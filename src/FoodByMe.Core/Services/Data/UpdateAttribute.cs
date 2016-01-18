using System;

namespace FoodByMe.Core.Services.Data
{
    public class UpdateAttribute : Attribute
    {
        public UpdateAttribute(int version, string tag)
        {
            if (version < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(version), "Version should be greater or equal to 1.");
            }
            if (string.IsNullOrEmpty(tag))
            {
                throw new ArgumentNullException(nameof(tag));
            }
            Version = version;
            Tag = tag;
        }

        public int Version { get; }

        public string Tag { get; }
    }
}
