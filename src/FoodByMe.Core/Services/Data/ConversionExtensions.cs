using System;
using System.Collections.Generic;
using System.Linq;
using FoodByMe.Core.Contracts;
using FoodByMe.Core.Contracts.Data;
using FoodByMe.Core.Services.Data.Types;

namespace FoodByMe.Core.Services.Data
{
    internal static class ConversionExtensions
    {
        public static RecipeRow ToRecipeRow(this Recipe recipe)
        {
            if (recipe == null)
            {
                throw new ArgumentNullException(nameof(recipe));
            }
            return new RecipeRow {Id = recipe.Id, Recipe = recipe.ToRecipeBlob()};
        }

        public static Recipe ToRecipe(this RecipeRow row, IReferenceBookService referenceBook)
        {
            if (row == null)
            {
                throw new ArgumentNullException(nameof(row));
            }
            if (referenceBook == null)
            {
                throw new ArgumentNullException(nameof(referenceBook));
            }
            var recipe = row.Recipe.ToRecipe(referenceBook);
            recipe.Id = row.Id;
            return recipe;
        }

        private static Recipe ToRecipe(this RecipeBlob recipeBlob, IReferenceBookService referenceBook)
        {
            var recipe = new Recipe
            {
                Title = recipeBlob.Title,
                ImageUri = recipeBlob.ImageUri,
                Notes = recipeBlob.Notes,
                IsFavorite = recipeBlob.IsFavorite,
                Category = recipeBlob.CategoryId > 0 
                    ? referenceBook.LookupCategory(recipeBlob.CategoryId) 
                    : null,
                CookingMinutes = recipeBlob.CookingMinutes,
                CookingSteps = recipeBlob.CookingSteps ?? new List<string>(),
                Ingredients = recipeBlob.Ingredients
                    ?.Select(x => ToIngredient(x, referenceBook))
                    .ToList() ?? new List<Ingredient>()
            };
            return recipe;
        }

        private static Ingredient ToIngredient(IngredientBlob ingredient, IReferenceBookService referenceBook)
        {
            return new Ingredient
            {
                Title = ingredient.Title,
                Measure = ingredient.MeasureId > 0 
                    ? referenceBook.LookupMeasure(ingredient.MeasureId)
                    : null,
                Quantity = ingredient.Quantity
            };
        }

        private static RecipeBlob ToRecipeBlob(this Recipe recipe)
        {
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
