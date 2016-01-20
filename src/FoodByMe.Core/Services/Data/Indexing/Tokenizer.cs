using System;
using System.Collections.Generic;
using System.Linq;

namespace FoodByMe.Core.Services.Data.Indexing
{
    internal static class Tokenizer
    {
        public static IEnumerable<string> Tokenize(string text)
        {
            var splitters = text.ToCharArray()
                .Where(x => !char.IsLetter(x))
                .ToArray();
            return text.Split(splitters, StringSplitOptions.RemoveEmptyEntries)
                .Where(x => x.Length > 2);
        }
    }
}
