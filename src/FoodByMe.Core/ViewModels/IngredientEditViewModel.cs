using System;
using System.Collections.Generic;
using System.Linq;
using FoodByMe.Core.Model;

namespace FoodByMe.Core.ViewModels
{
    public class IngredientEditViewModel : BaseViewModel
    {
        public List<Measure> Measures => Enum.GetValues(typeof(Measure))
            .Cast<int>()
            .Select(x => (Measure)x)
            .ToList();

    }
}