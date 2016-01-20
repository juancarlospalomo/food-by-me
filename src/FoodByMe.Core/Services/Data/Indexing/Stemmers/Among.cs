namespace FoodByMe.Core.Services.Data.Indexing.Stemmers
{
    internal class Among
    {
        public delegate bool boolDel();

        public readonly boolDel method; /* method to use if substring matches */
        public readonly int result; /* result of the lookup */
        public readonly char[] s; /* search string */

        public readonly int s_size; /* search string */
        public readonly int substring_i; /* index to longest matching substring */

        public Among(string s, int substring_i, int result, boolDel linkMethod)
        {
            s_size = s.Length;
            this.s = s.ToCharArray();
            this.substring_i = substring_i;
            this.result = result;
            method = linkMethod;
        }
    }
}