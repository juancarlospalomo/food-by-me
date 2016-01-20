/*
 *  Port of Snowball stemmers on C#
 *  Original stemmers can be found on http://snowball.tartarus.org
 *  Licence still BSD: http://snowball.tartarus.org/license.php
 *  
 *  Most of stemmers are ported from Java by Iveonik Systems ltd. (www.iveonik.com)
 */

namespace FoodByMe.Core.Services.Data.Indexing.Stemmers
{
    public interface IStemmer
    {
        string Stem(string s);
    }
}