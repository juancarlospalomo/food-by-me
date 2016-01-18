using System;
using System.Collections.Generic;
using System.Linq;
using FoodByMe.Core.Contracts;
using FoodByMe.Core.Services.Data.Types;

namespace FoodByMe.Core.Services.Data
{
    internal static class ConversionExtensions
    {
        public static RecipeTable ToRecipeTable(this Recipe recipe)
        {
            if (recipe == null)
            {
                throw new ArgumentNullException(nameof(recipe));
            }
            return new RecipeTable {Id = recipe.Id, Recipe = recipe.ToRecipeBlob()};
        }

        private static RecipeBlob ToRecipeBlob(this Recipe recipe)
        {
            if (recipe == null)
            {
                throw new ArgumentNullException(nameof(recipe));
            }
            var blob = new RecipeBlob
            {
                Title = recipe.Title,
                ImageUri = recipe.ImageUri,
                Notes = recipe.Notes,
                IsFavorite = recipe.IsFavorite,
                CategoryId = recipe.Category.Id,
                CookingMinutes = recipe.CookingMinutes,
                CookingSteps = recipe.CookingSteps ?? new List<string>(),
                Ingredients = recipe.Ingredients?.Select(ToIngredientBlob).ToList() ?? new List<IngredientBlob>()
            };
            return blob;
        }

        private static IngredientBlob ToIngredientBlob(Ingredient ingredient)
        {
            return new IngredientBlob
            {
                Title = ingredient.Title,
                MeasureId = ingredient.Measure.Id,
                Quantity = ingredient.Quantity
            };
        }
    }
}
