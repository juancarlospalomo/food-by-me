using System;
using System.Collections.Generic;

namespace FoodByMe.Core.ViewModels
{
    public class RecipeDisplayViewModel : BaseViewModel
    {
        private string _title;

        public RecipeDisplayViewModel()
        {
            var r = new Random();
            _title = $"Recipe ${r.Next()}";
            Ingredients = new List<IngredientDisplayViewModel>();
            CookingSteps = new List<CookingStepDisplayViewModel>();
        }

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                RaisePropertyChanged(() => Title);
            }
        }

        public List<IngredientDisplayViewModel> Ingredients { get; private set; }

        public List<CookingStepDisplayViewModel> CookingSteps { get; private set; }

        public override void Start()
        {
            base.Start();
            Ingredients.Clear();
            Ingredients.Add(new IngredientDisplayViewModel());
            Ingredients.Add(new IngredientDisplayViewModel());
            Ingredients.Add(new IngredientDisplayViewModel());
            CookingSteps.Clear();
            CookingSteps.Add(new CookingStepDisplayViewModel());
            CookingSteps.Add(new CookingStepDisplayViewModel());
            CookingSteps.Add(new CookingStepDisplayViewModel());
            CookingSteps.Add(new CookingStepDisplayViewModel());
        }
    }
}