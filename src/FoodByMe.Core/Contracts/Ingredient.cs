namespace FoodByMe.Core.Contracts
{
    public class Ingredient
    {
        public string Title { get; set; }

        public double Quantity { get; set; }

        public Measure Measure { get; set; }
    }
}
