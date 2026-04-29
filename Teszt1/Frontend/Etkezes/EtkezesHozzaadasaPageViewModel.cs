using Teszt1.Bakckend.Calsses;
using Teszt1.Bakckend.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teszt1.Frontend
{
    public partial class EtkezesHozzaadasaPageViewModel : ObservableObject
    {
        private readonly MealService _mealService;

        // Ezeket a változókat kötjük a beviteli mezőkhöz
        [ObservableProperty] private string mealName;
        [ObservableProperty] private string searchText;
        [ObservableProperty] private string quantity;
        [ObservableProperty] private Food selectedFood;

        // Ebbe a listába töltjük a keresés eredményét
        public ObservableCollection<Food> SearchResults { get; set; } = new ObservableCollection<Food>();

        public EtkezesHozzaadasaPageViewModel(MealService mealService)
        {
            _mealService = mealService;
        }


        public void LoadData()
        {
            SearchResults.Clear();
            var foods = _mealService.GetFood();
            foreach (var item in foods)
            {
                SearchResults.Add(item);
            }
        }
        [RelayCommand]
        public async Task SaveMealAsync()
        {
            // Ellenőrizzük, hogy minden adat megvan-e
            if (SelectedFood == null || string.IsNullOrWhiteSpace(MealName) || string.IsNullOrWhiteSpace(Quantity))
            {
                await App.Current.MainPage.DisplayAlert("Hiba", "Kérlek tölts ki minden mezőt!", "OK");
                return;
            }

            if (float.TryParse(Quantity, out float qty))
            {
                // Meghívjuk a service-t (itt a userId-t most fix 1-re írom, de később a bejelentkezett felhasználót használd)
                _mealService.AddMealWithEntry(1, MealName, SelectedFood.Id, qty);

                await App.Current.MainPage.DisplayAlert("Siker", "Étkezés rögzítve!", "OK");

                // Visszalépés az előző oldalra
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Hiba", "A mennyiség csak szám lehet!", "OK");
            }
        }

        //// Keresés gomb megnyomásakor fut le
        //[RelayCommand]
        //public void SearchFoods()
        //{
        //    if (string.IsNullOrWhiteSpace(SearchText)) return;

        //    var results = _databaseService.SearchFoods(SearchText);
        //    SearchResults.Clear();
        //    foreach (var food in results)
        //    {
        //        SearchResults.Add(food);
        //    }
        //}

        //// Mentés gomb megnyomásakor fut le
        //[RelayCommand]
        //public async Task SaveMealAsync()
        //{
        //    // Validálás (Ellenőrzés, hogy minden ki van-e töltve)
        //    if (SelectedFood == null || string.IsNullOrWhiteSpace(MealName) || string.IsNullOrWhiteSpace(Quantity))
        //    {
        //        await App.Current.MainPage.DisplayAlert("Hiba", "Kérlek adj meg egy nevet, válassz ki egy ételt és írj be egy mennyiséget!", "OK");
        //        return;
        //    }

        //    // Mennyiség számmá alakítása és mentés
        //    if (float.TryParse(Quantity, out float qty))
        //    {
        //        // A MySQL mentés meghívása
        //        _databaseService.AddMealWithFood(1, MealName, SelectedFood.Id, qty);

        //        // Visszanavigálás a Főoldalra
        //        await Shell.Current.GoToAsync("..");
        //    }
        //    else
        //    {
        //        await App.Current.MainPage.DisplayAlert("Hiba", "A mennyiség csak szám lehet!", "OK");
        //    }
        //}
    }
}
