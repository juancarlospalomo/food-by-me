using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using FoodByMe.Core.Contracts;
using FoodByMe.Core.Contracts.Data;
using FoodByMe.Core.Contracts.Messages;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using Plugin.Media;
using Plugin.Media.Abstractions;

namespace FoodByMe.Core.ViewModels
{
    public class RecipeEditViewModel : BaseViewModel
    {
        private const int DefaultIngredients = 2;
        private const int DefaultSteps = 1;

        private readonly IMvxMessenger _messenger;
        private readonly IRecipeCollectionService _recipeService;
        private readonly IPictureStorageService _pictureStorage;
        private IReadOnlyList<Measure> _measures;

        private string _notes;
        private string _title;
        private RecipeCategory _category;
        private int _cookingTimeSliderValue;
        private string _photoPath;
        
        public RecipeEditViewModel(IMvxMessenger messenger, 
            IRecipeCollectionService recipeService,
            IPictureStorageService pictureStorage)
        {
            if (recipeService == null)
            {
                throw new ArgumentNullException(nameof(recipeService));
            }
            if (pictureStorage == null)
            {
                throw new ArgumentNullException(nameof(pictureStorage));
            }
            if (messenger == null)
            {
                throw new ArgumentNullException(nameof(messenger));
            }
            _recipeService = recipeService;
            _pictureStorage = pictureStorage;
            _messenger = messenger;
            Ingredients = new ObservableCollection<IngredientEditViewModel>();
            Steps = new ObservableCollection<CookingStepEditViewModel>();
            Subscriptions.Add(_messenger.Subscribe<IngredientRemoving>(OnIngredientRemoving));
            Subscriptions.Add(_messenger.Subscribe<CookingStepRemoving>(OnCookingStepRemoving));
        }

        public int Id { get; set; }

        public bool IsFavorite { get; set; }
        
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                RaisePropertyChanged(() => Title);
            }
        }

        public string Notes
        {
            get { return _notes; }
            set
            {
                _notes = value;
                RaisePropertyChanged(() => Notes);
            }
        }

        public string PhotoPath
        {
            get { return _photoPath; }
            set
            {
                _photoPath = value;
                RaisePropertyChanged(() => PhotoPath);
            }
        }

        public IReadOnlyList<RecipeCategory> Categories { get; private set; }

        public RecipeCategory Category
        {
            get { return _category; }
            set
            {
                _category = value;
                RaisePropertyChanged();
            }
        }

        public int CookingTimeSliderValue
        {
            get { return _cookingTimeSliderValue; }
            set
            {
                _cookingTimeSliderValue = value;
                RaisePropertyChanged();
                RaisePropertyChanged(() => CookingTimeInMinutes);
            }
        }

        public int CookingTimeInMinutes
        {
            get { return CookingMinutesSliderConverter.ToMinutes(CookingTimeSliderValue); }
            set { CookingTimeSliderValue = CookingMinutesSliderConverter.FromMinutes(value); }
        }

        public ObservableCollection<IngredientEditViewModel> Ingredients { get; }

        public ObservableCollection<CookingStepEditViewModel> Steps { get; }

        public ICommand TakePhotoCommand => new MvxAsyncCommand(TakePhoto);

        public ICommand ChoosePhotoCommand => new MvxAsyncCommand(ChoosePhoto);

        public ICommand SaveRecipeCommand => new MvxAsyncCommand(SaveRecipe, CanSaveRecipe);

        public ICommand AddIngredientCommand => new MvxCommand(AddIngredient);

        public ICommand AddStepCommand => new MvxCommand(AddStep);

        public void Init(RecipeEditParameters parameters)
        {
            _measures = _recipeService.ReferenceBook.ListMeasures();
            Categories = _recipeService.ReferenceBook.ListCategories();
            if (parameters?.RecipeId != null && parameters?.RecipeId != default (int))
            {
                InitEditMode(parameters.RecipeId);
            }
            else
            {
                InitAddMode(parameters?.CategoryId);
            }
        }

        private void InitAddMode(int? categoryId)
        {
            Id = default(int);
            IsFavorite = false;
            Title = null;
            Category = categoryId == null
                ? Categories.FirstOrDefault()
                : Categories.FirstOrDefault(x => x.Id == categoryId.Value);
            Notes = null;
            CookingTimeSliderValue = 0;
            var steps = Enumerable.Range(0, DefaultSteps)
                .Select(i => new CookingStepEditViewModel(_messenger, i + 1))
                .ToList();
            Steps.Clear();
            foreach (var step in steps)
            {
                Steps.Add(step);
            }
            var ingredients = Enumerable.Range(0, DefaultIngredients)
                .Select(i => new IngredientEditViewModel(_messenger, _measures, i + 1))
                .ToList();
            Ingredients.Clear();
            foreach (var ingredient in ingredients)
            {
                Ingredients.Add(ingredient);
            }
        }

        private void InitEditMode(int recipeId)
        {
            var recipe = _recipeService.FindRecipeAsync(recipeId).Result;
            Id = recipeId;
            IsFavorite = recipe.IsFavorite;
            Title = recipe.Title;
            Category = recipe.Category;
            PhotoPath = recipe.ImageUri;
            CookingTimeInMinutes = recipe.CookingMinutes;
            Notes = recipe.Notes;
            Ingredients.Clear();
            for (var i = 0; i < recipe.Ingredients.Count; i++)
            {
                Ingredients.Add(new IngredientEditViewModel(_messenger, _measures, i + 1)
                {
                    Title = recipe.Ingredients[i].Title,
                    Measure = recipe.Ingredients[i].Measure,
                    Quantity = recipe.Ingredients[i].Quantity
                });
            }
            if (Ingredients.Count == 0)
            {
                Ingredients.Add(new IngredientEditViewModel(_messenger, _measures, 1));
            }
            Steps.Clear();
            for (var i = 0; i < recipe.CookingSteps.Count; i++)
            {
                Steps.Add(new CookingStepEditViewModel(_messenger, i + 1)
                {
                    Text = recipe.CookingSteps[i]
                });
            }
            if (Steps.Count == 0)
            {
                Steps.Add(new CookingStepEditViewModel(_messenger, 1));
            }
        }

        private bool CanSaveRecipe()
        {
            return !string.IsNullOrEmpty(Title);
        }

        private async Task SaveRecipe()
        {
            var recipe = this.ToRecipe();
            await _recipeService.SaveRecipeAsync(recipe);
            ShowViewModel<RecipeListViewModel>(new RecipeListParameters());
        }

        private async Task TakePhoto()
        {
            var options = new StoreCameraMediaOptions();
            var file = await CrossMedia.Current.TakePhotoAsync(options);
            await SetPhotoAsync(file);
        }

        private async Task ChoosePhoto()
        {
            var file = await CrossMedia.Current.PickPhotoAsync();
            await SetPhotoAsync(file);
        }

        private async Task SetPhotoAsync(MediaFile file)
        {
            if (file == null)
            {
                return;
            }
            if (!string.IsNullOrEmpty(PhotoPath))
            {
                await _pictureStorage.DeleteAsync(PhotoPath);
            }
            var path = await _pictureStorage.SaveAsync(file.Path);
            PhotoPath = path;
        }

        private void AddStep()
        {
            Steps.Add(new CookingStepEditViewModel(_messenger, Steps.Count + 1));
        }

        private void AddIngredient()
        {
            Ingredients.Add(new IngredientEditViewModel(_messenger, _measures, Ingredients.Count + 1));
        }

        private void OnCookingStepRemoving(CookingStepRemoving message)
        {
            var step = Steps.FirstOrDefault(x => x.Id == message.Id);
            if (step == null)
            {
                return;
            }
            Steps.Remove(step);
            UpdatePositions(Steps);
        }

        private void OnIngredientRemoving(IngredientRemoving message)
        {
            var ingredient = Ingredients.FirstOrDefault(x => x.Id == message.Id);
            if (ingredient == null)
            {
                return;
            }
            Ingredients.Remove(ingredient);
            UpdatePositions(Ingredients);
        }

        private static void UpdatePositions(IReadOnlyList<IPositionable> collection)
        {
            for (var i = 0; i < collection.Count; i++)
            {
                collection[i].Position = i + 1;
            }
        }
    }
}