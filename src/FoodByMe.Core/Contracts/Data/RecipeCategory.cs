namespace FoodByMe.Core.Contracts.Data
{
    public class RecipeCategory
    {
        public int Id { get; internal set; }

        public string Title { get; internal set; }

        public override string ToString()
        {
            return Title;
        }
    }
}
