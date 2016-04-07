using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using FoodByMe.Core.Contracts.Data;
using FoodByMe.Core.Services.Data.Types;

namespace FoodByMe.Core.Services.Data.Indexing
{
    internal static class FieldExtractor
    {
        public static class Tokens
        {
            public const string FavoriteTag = "Favorite";
        }

        public static List<RecipeTextFieldRow> Extract(Recipe recipe)
        {
            if (recipe == null)
            {
                throw new ArgumentNullException(nameof(recipe));
            }
            var fields = new List<RecipeTextFieldRow>();
            if (!string.IsNullOrWhiteSpace(recipe.Title))
            {
                fields.Add(new RecipeTextFieldRow
                {
                    Type = RecipeTextType.Title,
                    Value = recipe.Title.Trim()
                });
            }
            if (!string.IsNullOrWhiteSpace(recipe.Notes))
            {
                fields.Add(new RecipeTextFieldRow
                {
                    Type = RecipeTextType.Notes,
                    Value = recipe.Notes.Trim()
                });
            }
            if (recipe.Category != null)
            {
                fields.Add(new RecipeTextFieldRow
                {
                    Type = RecipeTextType.CategoryId,
                    Value = recipe.Category.Id.ToString(CultureInfo.InvariantCulture),
                    IsSearchable = false
                });
            }
            if (recipe.IsFavorite)
            {
                fields.Add(new RecipeTextFieldRow
                {
                    Type = RecipeTextType.Tags,
                    Value = Tokens.FavoriteTag
                });
            }
            var steps = recipe.CookingSteps
                ?.Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList();
            if (steps?.Count > 0)
            {
                fields.Add(new RecipeTextFieldRow
                {
                    Type = RecipeTextType.CookingSteps,
                    Value = string.Join(Environment.NewLine, steps).Trim()
                });
            }
            var ingredients = recipe.Ingredients
                ?.Where(x => !string.IsNullOrWhiteSpace(x.Title))
                .ToList();
            if (ingredients?.Count > 0)
            {
                fields.AddRange(ingredients.Select(x => new RecipeTextFieldRow
                {
                    Type = RecipeTextType.Ingredient,
                    Value = x.Title.Trim()
                }));
            }
            fields.Add(new RecipeTextFieldRow
            {
                IsSearchable = false,
                Type = RecipeTextType.CreatedDate,
                Value = recipe.CreatedAt.ToString("O")
            });
            fields.Add(new RecipeTextFieldRow
            {
                IsSearchable = false,
                Type = RecipeTextType.LastModifiedDate,
                Value = recipe.LastModifiedAt.ToString("O")
            });
            foreach (var field in fields)
            {
                field.RecipeId = recipe.Id;
            }
            return fields.ToList();
        } 
    }
}
