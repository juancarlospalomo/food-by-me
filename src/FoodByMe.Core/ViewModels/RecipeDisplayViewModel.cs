using System.Collections.Generic;

namespace FoodByMe.Core.ViewModels
{
    public class RecipeDisplayViewModel : BaseViewModel
    {
        public RecipeDisplayViewModel()
        {
            Ingredients = new List<IngredientDisplayViewModel>();
            CookingSteps = new List<CookingStepDisplayViewModel>();
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public int CookingMinutes { get; set; }

        public string Category { get; set; }

        public string ImageUri { get; set; }

        public string Notes { get; set; }

        public List<IngredientDisplayViewModel> Ingredients { get; set; }

        public List<CookingStepDisplayViewModel> CookingSteps { get; set; }
    }
}