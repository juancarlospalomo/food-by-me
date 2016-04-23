using System.Collections.Generic;

namespace FoodByMe.Core.ViewModels
{
    public class RecipeDisplayViewModel : BaseViewModel
    {
        private string _title;
        private int _cookingMinutes;
        private string _category;
        private string _imageUri;
        private string _notes;
        private List<IngredientDisplayViewModel> _ingredients;
        private List<CookingStepDisplayViewModel> _cookingSteps;  

        public RecipeDisplayViewModel()
        {
            Ingredients = new List<IngredientDisplayViewModel>();
            CookingSteps = new List<CookingStepDisplayViewModel>();
        }

        public int Id { get; set; }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value, nameof(Title)); }
        }

        public int CookingMinutes
        {
            get { return _cookingMinutes; }
            set { SetProperty(ref _cookingMinutes, value, nameof(CookingMinutes)); }
        }

        public string Category
        {
            get { return _category; }
            set { SetProperty(ref _category, value, nameof(Category)); }
        }

        public string ImageUri
        {
            get { return _imageUri; }
            set { SetProperty(ref _imageUri, value, nameof(ImageUri)); }
        }

        public string Notes
        {
            get { return _notes; }
            set { SetProperty(ref _notes, value, nameof(Notes)); }
        }

        public bool HasNotes => !string.IsNullOrEmpty(Notes);

        public List<IngredientDisplayViewModel> Ingredients
        {
            get { return _ingredients; }
            set { SetProperty(ref _ingredients, value, nameof(Ingredients)); }
        }

        public bool IngredientsEmpty => Ingredients.Count == 0;

        public List<CookingStepDisplayViewModel> CookingSteps
        {
            get { return _cookingSteps; }
            set { SetProperty(ref _cookingSteps, value, nameof(CookingSteps)); }
        }

        public bool CookingStepsEmpty => CookingSteps.Count == 0;
    }
}