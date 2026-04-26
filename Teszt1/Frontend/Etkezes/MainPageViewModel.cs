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
    public partial class MainPageViewModel : ObservableObject
    {
        private readonly DatabaseService _databaseService;

        // --- Kalória ---
        [ObservableProperty] private double calorieProgress;
        [ObservableProperty] private string? calorieText;

        // --- Makrók ---
        [ObservableProperty] private double carbsProgress;
        [ObservableProperty] private string? carbsText;

        [ObservableProperty] private double proteinProgress;
        [ObservableProperty] private string? proteinText;

        [ObservableProperty] private double fatProgress;
        [ObservableProperty] private string? fatText;

        // Így biztosan magyarul írja ki a hónapot és a napot, a megfelelő formátumban!
        public string MaiDatum => DateTime.Now.ToString("yyyy. MMMM dd. (dddd)", new System.Globalization.CultureInfo("hu-HU"));

        // Lista az utolsó étkezéseknek
        public ObservableCollection<string> RecentMeals { get; set; } = new ObservableCollection<string>();

        public MainPageViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        // Ez fut le, amikor megnyitod a Főoldalt
        public void LoadData()
        {
            // 1. KALÓRIA LEKÉRDEZÉS (Biztonságosan)
            float bevittKaloria = 0f;
            float celKaloria = 2000f; // Később ezt a User táblából (TDEE) vesszük

            try
            {
                bevittKaloria = _databaseService.GetTodayCalories(1);
            }
            catch
            {
                // Ha a Data is Null hiba jön (mert ma még nem ettél), akkor a bevittKaloria marad 0!
            }

            CalorieProgress = Math.Min(bevittKaloria / celKaloria, 1.0);
            CalorieText = $"Kalóriacél / Összkalória: {bevittKaloria} / {celKaloria} kcal";

            // 2. MAKRÓK (Ideiglenesen beállítjuk őket, amíg nem írjuk meg az SQL-t a makrókra is)
            CarbsText = "Szénhidrát: 0g / 250g";
            CarbsProgress = 0.0;

            ProteinText = "Fehérje: 0g / 150g";
            ProteinProgress = 0.0;

            FatText = "Zsír: 0g / 70g";
            FatProgress = 0.0;

            // 3. UTOLSÓ 3 ÉTKEZÉS (Biztonságosan)
            RecentMeals.Clear();
            try
            {
                var meals = _databaseService.GetLastThreeMeals(1);

                if (meals.Count == 0)
                {
                    RecentMeals.Add("Még nincs rögzített étkezés mára.");
                }
                else
                {
                    foreach (var meal in meals)
                    {
                        RecentMeals.Add("• " + meal);
                    }
                }
            }
            catch
            {
                // Ha hiányzik a 'name' oszlop a MySQL-ből, akkor ne fekete hibaablak ugorjon fel,
                // hanem csak írja ki a listába, hogy nem sikerült betölteni.
                RecentMeals.Add("Hiba a betöltéskor (Ellenőrizd a MySQL-t!).");
            }
        }

        // Gombnyomásra átvisz az Étkezés hozzáadására
        [RelayCommand]
        public async Task GoToEtkezesAsync()
        {
            await Shell.Current.GoToAsync("EtkezesHozzaadasaPage");
        }
    }
}
