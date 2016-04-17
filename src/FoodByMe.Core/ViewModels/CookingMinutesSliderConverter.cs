namespace FoodByMe.Core.ViewModels
{
    internal static class CookingMinutesSliderConverter
    {
        private const int Min = 1;
        private const int Threshold = 20;
        private const int Multiplier = 5;

        public static int ToMinutes(int value)
        {
            var x = value + Min;
            x = x <= Threshold
                ? x
                : Threshold + (x - Threshold) * Multiplier;
            return x;
        }

        public static int FromMinutes(int value)
        {
            return value <= Threshold
                ? value - Min
                : (value - Threshold)/Multiplier + Threshold - Min;
        }
    }
}
