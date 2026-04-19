using Teszt1.Bakckend.Calsses;
namespace Teszt1
{
    public partial class EtkezesHozzaadasaPage : ContentPage
    {
        //DatabaseService _dbService = new DatabaseService();
        //Food selectedFood;

        //public EtkezesHozzaadasaPage()
        //{
        //    InitializeComponent();
        //}

        //private void OnSearchPressed(object sender, EventArgs e)
        //{
        //    FoodResultList.ItemsSource = _dbService.SearchFoods(FoodSearchBar.Text);
        //}

        //private void OnFoodSelected(object sender, SelectedItemChangedEventArgs e)
        //{
        //    selectedFood = e.SelectedItem as Food;
        //}

        //private async void OnSaveClicked(object sender, EventArgs e)
        //{
        //    if (selectedFood == null || string.IsNullOrEmpty(MealNameEntry.Text))
        //    {
        //        await DisplayAlert("Hiba", "Válassz ételt és adj nevet az étkezésnek!", "OK");
        //        return;
        //    }

        //    float qty = float.Parse(QtyEntry.Text);
        //    _dbService.AddMealWithFood(1, MealNameEntry.Text, selectedFood.Id, qty);

        //    await Navigation.PopAsync(); // Vissza a fõoldalra
        //}
    }
}