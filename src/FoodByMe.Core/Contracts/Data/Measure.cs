namespace FoodByMe.Core.Contracts.Data
{
    public class Measure
    {
        public int Id { get; internal set; }

        public string Title { get; internal set; }

        public string ShortTitle { get; internal set; }

        public override string ToString()
        {
            return ShortTitle;
        }
    }
}
