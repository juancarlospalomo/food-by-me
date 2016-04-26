using System;
using System.Collections.Generic;
using System.Linq;
using FoodByMe.Core.Contracts;
using FoodByMe.Core.Contracts.Data;
using MvvmCross.Plugins.Messenger;

namespace FoodByMe.Core.ViewModels
{
    public static class ConversionExtensions
    {
        public static Recipe ToRecipe(this RecipeEditViewModel vm)
        {
            if (vm == null)
            {
                throw new ArgumentNullException(nameof(vm));
            }
            var steps = vm.Steps
                .OrderBy(x => x.Position)
                .Where(x => !string.IsNullOrWhiteSpace(x.Text))
                .Select(x => x.Text)
                .ToList();
            var ingredients = vm.Ingredients
                .OrderBy(x => x.Position)
                .Where(x => !string.IsNullOrWhiteSpace(x.Title))
                .Select(x => new Ingredient
                {
                    Measure = x.Measure,
                    Quantity = x.Quantity,
                    Title = x.Title
                })
                .ToList();
            return new Recipe
            {
                Id = vm.Id,
                IsFavorite = vm.IsFavorite,
                Title = vm.Title,
                ImageUri = vm.PhotoPath,
                Category = vm.Category,
                CookingMinutes = vm.CookingTimeInMinutes,
                CookingSteps = steps,
                Ingredients = ingredients,
                Notes = vm.Notes
            };
        }

        public static RecipeDisplayViewModel ToRecipeDisplayViewModel(this Recipe recipe)
        {
            return new RecipeDisplayViewModel()
            {
                Id = recipe.Id,
                Title = recipe.Title,
                Category = recipe.Category.Title,
                CookingMinutes = recipe.CookingMinutes,
                ImageUri = recipe.ImageUri,
                Notes = recipe.Notes,
                CookingSteps = recipe.CookingSteps.Select(ToCookingStepDisplayViewModel).ToList(),
                Ingredients = recipe.Ingredients.Select(ToIngredientDisplayViewModel).ToList()
            };
        }

        private static IngredientDisplayViewModel ToIngredientDisplayViewModel(Ingredient ingredient, int index)
        {
            return new IngredientDisplayViewModel
            {
                Position = index + 1,
                Measure = ingredient.Measure,
                Quantity = ingredient.Quantity,
                Title = ingredient.Title
            };
        }

        private static CookingStepDisplayViewModel ToCookingStepDisplayViewModel(string text, int index)
        {
            return new CookingStepDisplayViewModel {Position = index + 1, Text = text};
        }

        public static RecipeListItemViewModel ToRecipeListItemViewModel(this Recipe recipe, IMvxMessenger messenger)
        {
            var description = recipe.Notes ?? string.Join(", ", recipe.Ingredients.Select(GetDescriptionFromIngredient));
            return new RecipeListItemViewModel(messenger)
            {
                Id = recipe.Id,
                IsFavorite = recipe.IsFavorite,
                Title = recipe.Title,
                Description = description,
                ImageUrl = recipe.ImageUri ?? GetDefaultImagePath(recipe.Category.Id),
                CookingMinutes = recipe.CookingMinutes
            };
        }

        public static RecipeListParameters ToRecipeListParameters(this RecipeQuery query)
        {
            return new RecipeListParameters
            {
                CategoryId = query.CategoryId.GetValueOrDefault(0),
                CategorySelected = query.CategoryId != null,
                IsFavoriteSelected = query.OnlyFavorite,
                SearchTerm = query.SearchTerm
            };
        }

        public static RecipeQuery ToQuery(this RecipeListParameters parameters)
        {
            return new RecipeQuery
            {
                CategoryId = parameters.CategorySelected ? parameters.CategoryId : (int?) null,
                OnlyFavorite = parameters.IsFavoriteSelected,
                SearchTerm = parameters.SearchTerm
            };
        }

        private static string GetDefaultImagePath(int id)
        {
            switch (id)
            {
                case Constants.Categories.Appetizers:
                    return "res:ic_app_appetizers";
                case Constants.Categories.Baking:
                    return "res:ic_app_bakery";
                case Constants.Categories.Desserts:
                    return "res:ic_app_desserts";
                case Constants.Categories.Dinner:
                    return "res:ic_app_hot_dishes";
                case Constants.Categories.Drinks:
                    return "res:ic_app_drinks";
                case Constants.Categories.Other:
                    return "res:ic_app_other";
                case Constants.Categories.Salads:
                    return "res:ic_app_salads";
                default:
                    return "res:ic_app_other";
            }
        }

        private static string GetDescriptionFromIngredient(Ingredient ingredient)
        {
            var parts = new List<string> {ingredient.Title};
            if (ingredient.Quantity != null)
            {
                parts.Add($"{ingredient.Quantity}{ingredient.Measure.ShortTitle}");
            }
            return string.Join(" ", parts);
        }
    }
}
