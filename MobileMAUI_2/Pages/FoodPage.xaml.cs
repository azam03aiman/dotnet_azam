using MobileMAUI_2.Model;
using MobileMAUI_2.Services;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MobileMAUI_2.Pages
{
    public partial class FoodPage : ContentPage
    {
        private ObservableCollection<Food> foods;
        private Food selectedFood;
        private readonly DatabaseService _databaseService;  // Add this field

        public FoodPage()
        {
            InitializeComponent();
            _databaseService = new DatabaseService();  // Initialize the DatabaseService
            foods = new ObservableCollection<Food>();
            FoodCollectionView.ItemsSource = foods;

            LoadFoods();  // Load foods from database on page load
        }

        // Load all foods from the database
        private async Task LoadFoods()
        {
            var foodList = await _databaseService.GetAllFoodAsync();
            foods.Clear();  // Clear any existing items
            foreach (var food in foodList)
            {
                foods.Add(food);  // Add food items from database
            }
        }

        // Add Food Button Clicked
        private async void OnAddFoodClicked(object sender, EventArgs e)
        {
            FoodFormFrame.IsVisible = true;  // Show the form
        }

        // Save Button Clicked (Add/Edit)
        private async void OnSaveFoodClicked(object sender, EventArgs e)
        {
            if (selectedFood == null)  // New Food
            {
                var food = new Food
                {
                    Name = FoodNameEntry.Text,
                    Description = FoodDescriptionEntry.Text,
                    Rating = int.Parse(RatingEntry.Text)
                };
                foods.Add(food);  // Add to the ObservableCollection
                await _databaseService.AddFoodAsync(food);  // Save to the database
            }
            else  // Edit Existing Food
            {
                selectedFood.Name = FoodNameEntry.Text;
                selectedFood.Description = FoodDescriptionEntry.Text;
                selectedFood.Rating = int.Parse(RatingEntry.Text);
                await _databaseService.UpdateFoodAsync(selectedFood);  // Update the database
            }

            ClearForm();
            await LoadFoods();  // Refresh the food list after saving
        }

        // Cancel Button Clicked
        private void OnCancelFoodClicked(object sender, EventArgs e)
        {
            ClearForm();
        }

        // Edit Button Clicked
        private void OnEditFoodClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            selectedFood = button?.BindingContext as Food;

            if (selectedFood != null)
            {
                FoodNameEntry.Text = selectedFood.Name;
                FoodDescriptionEntry.Text = selectedFood.Description;
                RatingEntry.Text = selectedFood.Rating.ToString();
                FoodFormFrame.IsVisible = true;  // Show the form to edit
            }
        }

        // Delete Button Clicked
        private async void OnDeleteFoodClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var food = button?.BindingContext as Food;

            if (food != null)
            {
                foods.Remove(food);  // Remove from ObservableCollection
                await _databaseService.DeleteFoodAsync(food);  // Delete from the database
            }
        }

        // On Selection Changed
        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Count > 0)
            {
                var selectedFood = e.CurrentSelection[0] as Food;
                // Handle selection logic if needed
            }
        }

        // Clear Form Fields
        private void ClearForm()
        {
            FoodNameEntry.Text = "";
            FoodDescriptionEntry.Text = "";
            RatingEntry.Text = "";
            FoodFormFrame.IsVisible = false;
            selectedFood = null;
        }
    }
}
