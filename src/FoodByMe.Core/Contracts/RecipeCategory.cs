namespace FoodByMe.Core.Contracts
{
    public class RecipeCategory
    {
        internal RecipeCategory()
        {
        }

        public int Id { get; internal set; }

        public string Title { get; internal set; }

        public override string ToString()
        {
            return Title;
        }
    }
}
