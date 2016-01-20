using System;
using System.Linq;
using FoodByMe.Core.Framework;

namespace FoodByMe.Core.Services.Data.Indexing
{
    internal class QueryParser
    {
        private readonly ICultureProvider _cultureProvider;

        public QueryParser(ICultureProvider cultureProvider)
        {
            if (cultureProvider == null)
            {
                throw new ArgumentNullException(nameof(cultureProvider));
            }
            _cultureProvider = cultureProvider;
        }

        public string Parse(string query)
        {
            if (string.IsNullOrWhiteSpace(query) || query.Length < 3)
            {
                return null;
            }
            var stemmer = StemmerFactory.Create(_cultureProvider.Culture);
            var tokens = Tokenizer.Tokenize(query).Select(stemmer.Stem).ToArray();
            var q = string.Join(" ", tokens);
            return q;
        }
    }
}
