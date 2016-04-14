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
        private IReadOnlyList<Measure> _measures;

        private string _notes;
        private string _title;
        private RecipeCategory _category;
        private int _cookingTimeSliderValue;
        private string _photoPath;
        
        public RecipeEditViewModel(IMvxMessenger messenger, IRecipeCollectionService recipeService)
        {
            if (recipeService == null)
            {
                throw new ArgumentNullException(nameof(recipeService));
            }
            if (messenger == null)
            {
                throw new ArgumentNullException(nameof(messenger));
            }
            _recipeService = recipeService;
            _messenger = messenger;
            Ingredients = new ObservableCollection<IngredientEditViewModel>();
            Steps = new ObservableCollection<CookingStepEditViewModel>();
            Subscriptions.Add(_messenger.Subscribe<IngredientRemoving>(OnIngredientRemoving));
            Subscriptions.Add(_messenger.Subscribe<CookingStepRemoving>(OnCookingStepRemoving));
        }
        
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

        public int CookingTimeInMinutes => CalculateCookingTimeInMinutes(_cookingTimeSliderValue);

        public ObservableCollection<IngredientEditViewModel> Ingredients { get; }

        public ObservableCollection<CookingStepEditViewModel> Steps { get; }

        public ICommand TakePhotoCommand => new MvxAsyncCommand(TakePhoto);

        public ICommand ChoosePhotoCommand => new MvxAsyncCommand(ChoosePhoto);

        public ICommand SaveRecipeCommand => new MvxAsyncCommand(SaveRecipe, CanSaveRecipe);

        public ICommand AddIngredientCommand => new MvxCommand(AddIngredient);

        public ICommand AddStepCommand => new MvxCommand(AddStep);

        public override void Start()
        {
            base.Start();
            _measures = _recipeService.ReferenceBook.ListMeasures();
            Categories = _recipeService.ReferenceBook.ListCategories();
            Category = Categories.FirstOrDefault();
            Ingredients.Clear();
            Steps.Clear();
            for (var i = 0; i < DefaultIngredients; i++)
            {
                AddIngredient();
            }
            for (var i = 0; i < DefaultSteps; i++)
            {
                AddStep();
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
            ShowViewModel<RecipeListViewModel>();
        }

        private async Task TakePhoto()
        {
            var options = new StoreCameraMediaOptions();
            var file = await CrossMedia.Current.TakePhotoAsync(options);
            SetPhoto(file);
        }

        private async Task ChoosePhoto()
        {
            var file = await CrossMedia.Current.PickPhotoAsync();
            SetPhoto(file);
        }

        private void SetPhoto(MediaFile file)
        {
            if (file == null)
            {
                return;
            }
            PhotoPath = file.Path;
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

        private static int CalculateCookingTimeInMinutes(int sliderValue)
        {
            const int min = 1;
            const int threshold = 20;
            const int multiplier = 5;
            sliderValue = sliderValue + min;
            var value = sliderValue <= threshold
                ? sliderValue
                : threshold + (sliderValue - threshold)*multiplier;
            return value;
        }
    }
}