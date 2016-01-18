using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using FoodByMe.Core.Contracts;
using FoodByMe.Core.Services.Data.Types;

namespace FoodByMe.Core.Services.Data
{
    internal static class RecipeIndexer
    {
        public static class Tokens
        {
            public const string FavoriteTag = "Favorite";
        }

        public static List<RecipeTextFieldTable> CreateIndices(Recipe recipe)
        {
            if (recipe == null)
            {
                throw new ArgumentNullException(nameof(recipe));
            }
            var fields = new List<RecipeTextFieldTable>
            {
                new RecipeTextFieldTable
                {
                    RecipeId = recipe.Id,
                    Type = RecipeFieldType.Title,
                    Value = recipe.Title
                },
                new RecipeTextFieldTable
                {
                    RecipeId = recipe.Id,
                    Type = RecipeFieldType.Notes,
                    Value = recipe.Notes
                },
                new RecipeTextFieldTable
                {
                    RecipeId = recipe.Id,
                    Type = RecipeFieldType.Category,
                    Value = recipe.Category.Id.ToString(CultureInfo.InvariantCulture)
                },
                new RecipeTextFieldTable
                {
                    RecipeId = recipe.Id,
                    Type = RecipeFieldType.Tags,
                    Value = Tokens.FavoriteTag
                }
            };
            if (recipe.CookingSteps?.Count > 0)
            {
                fields.Add(new RecipeTextFieldTable
                {
                    RecipeId = recipe.Id,
                    Type = RecipeFieldType.CookingSteps,
                    Value = string.Join(Environment.NewLine, recipe.CookingSteps)
                });
            }
            if (recipe.Ingredients?.Count > 0)
            {
                fields.AddRange(recipe.Ingredients.Select(x => new RecipeTextFieldTable
                {
                    RecipeId = recipe.Id,
                    Type = RecipeFieldType.Ingredient,
                    Value = x.Title
                }));
            }
            return fields.Where(x => !string.IsNullOrWhiteSpace(x.Value)).ToList();
        } 
    }
}
