using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FoodByMe.Core.Contracts;
using FoodByMe.Core.Contracts.Data;
using FoodByMe.Core.Contracts.Messages;
using MvvmCross.Plugins.Messenger;

namespace FoodByMe.Core.Services
{
    public class RecipeCollectionService : IRecipeCollectionService
    {
        private readonly IRecipePersistenceService _recipePersistenceService;
        private readonly ISearchService _searchService;
        private readonly IMvxMessenger _messenger;

        public RecipeCollectionService(IRecipePersistenceService recipePersistenceService, ISearchService searchService, IMvxMessenger messenger)
        {
            if (recipePersistenceService == null)
            {
                throw new ArgumentNullException(nameof(recipePersistenceService));
            }
            if (searchService == null)
            {
                throw new ArgumentNullException(nameof(searchService));
            }
            if (messenger == null)
            {
                throw new ArgumentNullException(nameof(messenger));
            }
            _recipePersistenceService = recipePersistenceService;
            _searchService = searchService;
            _messenger = messenger;
        }

        public Task<List<Recipe>> SearchRecipesAsync(RecipeQuery query)
        {
            query = query ?? new RecipeQuery();
            return Task.Run(() => _searchService.SearchRecipes(query));
        }

        public Task<Recipe> FindRecipeAsync(int recipeId)
        {
            if (recipeId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(recipeId));
            }
            return Task.Run(() => _recipePersistenceService.FindRecipe(recipeId));
        }

        public Task SaveRecipeAsync(Recipe recipe)
        {
            if (recipe == null)
            {
                throw new ArgumentNullException(nameof(recipe));
            }
            return Task.Run(() =>
            {
                var add = recipe.Id == default(int);
                if (add)
                {
                    recipe.CreatedAt = new DateTime();
                }
                recipe.LastModifiedAt = new DateTime();
                recipe = _recipePersistenceService.SaveRecipe(recipe);
                var message = add ? (MvxMessage) new RecipeAdded(this, recipe) : new RecipeUpdated(this, recipe);
                _messenger.Publish(message);
            });
        }

        public Task RemoveRecipeAsync(int recipeId)
        {
            return Task.Run(() =>
            {
                _recipePersistenceService.RemoveRecipe(recipeId);
                _messenger.Publish(new RecipeRemoved(this, recipeId));
            });
        }

        public async Task ToggleRecipeFavoriteTagAsync(int recipeId, bool isFavorite)
        {
            if (recipeId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(recipeId));
            }
            var recipe = await FindRecipeAsync(recipeId).ConfigureAwait(false);
            recipe.IsFavorite = isFavorite;
            _recipePersistenceService.SaveRecipe(recipe);
            _messenger.Publish(new RecipeFavoriteTagChanged(this, recipeId, isFavorite));
        }

        public IReferenceBookService ReferenceBook => _searchService;
    }
}
