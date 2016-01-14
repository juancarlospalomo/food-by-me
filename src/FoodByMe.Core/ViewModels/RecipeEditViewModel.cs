using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Cirrious.MvvmCross.ViewModels;
using FoodByMe.Core.Model;

namespace FoodByMe.Core.ViewModels
{
    public class RecipeEditViewModel : BaseViewModel
    {
        public RecipeEditViewModel()
        {
            Ingredients = new ObservableCollection<IngredientEditViewModel>();
            Steps = new ObservableCollection<CookingStepEditViewModel>();
        }

        public ObservableCollection<IngredientEditViewModel> Ingredients { get; }

        public ObservableCollection<CookingStepEditViewModel> Steps { get; }

        public List<RecipeCategory> Categories => Enum.GetValues(typeof (RecipeCategory))
            .Cast<int>()
            .Select(x => (RecipeCategory) x)
            .ToList();

        public ICommand AddIngredientCommand => new MvxCommand(AddIngredient);

        public ICommand AddStepCommand => new MvxCommand(AddStep);

        public override void Start()
        {
            base.Start();
            Ingredients.Clear();
            for (var i = 0; i < 2; i++)
            {
                Ingredients.Add(new IngredientEditViewModel());
            }
            Steps.Clear();
            Steps.Add(new CookingStepEditViewModel());
        }

        private void AddStep()
        {
            Steps.Add(new CookingStepEditViewModel());
        }

        private void AddIngredient()
        {
            Ingredients.Add(new IngredientEditViewModel());
        }
    }
}