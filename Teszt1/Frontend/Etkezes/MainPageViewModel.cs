using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Teszt1.Bakckend.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teszt1.Bakckend.Database;

namespace Teszt1.Frontend
{
    public partial class MainPageViewModel : ObservableObject
    {
        private readonly FitnessDbContext _databaseService;
        private readonly MealService _mealService;

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

        public MainPageViewModel(FitnessDbContext databaseService, MealService mealService)
        {
            _databaseService = databaseService;
            _mealService = mealService;
        }

        
        public void LoadData()
        {
            // 1. Felhasználó azonosítója (példaként 1, később a bejelentkezésből jön)
            int userId = 1;

            // 2. Makrók és kalóriák lekérése a Service-ből
            // Ez a metódus végzi el a matekot a FitnessDbContext adatai alapján
            var summary = _mealService.GetTodayNutritionSummary(userId);

            // 3. Célértékek beállítása (Ezeket később érdemes a User táblából venni)
            float goalKcal = 2000;
            float goalCarbs = 250;
            float goalProtein = 150;
            float goalFat = 70;

            // 4. UI Változók frissítése (A ProgressBar-ok és Feliratok számára)

            // Kalória adatok
            CalorieText = $"{Math.Round(summary.Calories)} / {goalKcal} kcal";
            CalorieProgress = summary.Calories / goalKcal;

            // Szénhidrát (F1 formátum = 1 tizedesjegy, pl: 45.2g)
            CarbsText = $"Szénhidrát: {summary.Carbs:F1}g / {goalCarbs}g";
            CarbsProgress = summary.Carbs / goalCarbs;

            // Fehérje
            ProteinText = $"Fehérje: {summary.Protein:F1}g / {goalProtein}g";
            ProteinProgress = summary.Protein / goalProtein;

            // Zsír
            FatText = $"Zsír: {summary.Fat:F1}g / {goalFat}g";
            FatProgress = summary.Fat / goalFat;

            // 5. Utolsó 3 étkezés frissítése a listában
            UpdateRecentMealsList(userId);
        }
        private void UpdateRecentMealsList(int userId)
        {
            RecentMeals.Clear();
            try
            {
                var meals = _mealService.GetLastThreeMealsFormatted(userId);

                if (meals == null || meals.Count == 0)
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
            catch (Exception)
            {
                RecentMeals.Add("Hiba az étkezések betöltésekor.");
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
