using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teszt1.Bakckend.Calsses;
using Teszt1.Bakckend.Interface;

namespace Teszt1.Bakckend.Services
{
    public class MealService
    {
        private readonly IFoodDataProvider foodDataProvider;
        private readonly IMealDataProvider mealDataProvider;
        private readonly IMealEntryDataProvider mealEntryDataProvider;
        private readonly IUserDataProvider userDataProvider;

        public MealService(IFoodDataProvider foodDataProvider, IUserDataProvider userDataProvider, IMealDataProvider mealDataProvider, IMealEntryDataProvider mealEntryDataProvider)
        {
            this.foodDataProvider = foodDataProvider;
            this.mealDataProvider = mealDataProvider;
            this.mealEntryDataProvider = mealEntryDataProvider;
            this.userDataProvider = userDataProvider;
        }

        
        public Macros GetTodayNutritionSummary(int userId)
        {
            var summary = new Macros();

            // 1. Mai étkezések lekérése
            var todayMeals = mealDataProvider.GetMeals(userId)
                .Where(m => m.Date.Date == DateTime.Today)
                .ToList();

            foreach (var meal in todayMeals)
            {
                // 2. Az étkezéshez tartozó tételek
                var entries = mealEntryDataProvider.GetMealEntries(meal.Id);

                foreach (var entry in entries)
                {
                    // 3. Étel adatai a kalóriához és makrókhoz
                    var food = foodDataProvider.GetFoods().FirstOrDefault(f => f.Id == entry.Food_Id);

                    if (food != null)
                    {
                        // Kiszámoljuk az arányt (mivel az adatok 100g-ra vonatkoznak)
                        float ratio = entry.Qty / 100f;

                        summary.Calories += food.Kcal * ratio;
                        summary.Protein += food.Protein * ratio;
                        summary.Carbs += food.Carbs * ratio;
                        summary.Fat += food.Fat * ratio;
                    }
                }
            }

            return summary;
        }
        public List<string> GetLastThreeMealsFormatted(int userId)
        {
            // 1. A MealDataProvider-en keresztül lekérjük a felhasználó étkezéseit
            // (A GetMeals-t érdemes kiegészíteni a DataProviderben, hogy rendezve adja vissza, 
            // vagy itt rendezzük)
            var allMeals = mealDataProvider.GetMeals(userId)
                .OrderByDescending(m => m.Date)
                .Take(3)
                .ToList();

            List<string> formattedList = new List<string>();

            foreach (var meal in allMeals)
            {
                float mealTotalKcal = 0;

                // 2. Lekérjük az étkezéshez tartozó tételeket a MealEntryDataProvider-rel
                var entries = mealEntryDataProvider.GetMealEntries(meal.Id);

                foreach (var entry in entries)
                {
                    // 3. Megkeressük az ételt a FoodDataProvider-rel
                    // (Itt látszik, miért jó, ha van GetFoodById az interfészben)
                    var food = foodDataProvider.GetFoods().FirstOrDefault(f => f.Id == entry.Food_Id);

                    if (food != null)
                    {
                        mealTotalKcal += (food.Kcal * entry.Qty) / 100f;
                    }
                }

                string name = string.IsNullOrEmpty(meal.Name) ? "Étkezés" : meal.Name;
                formattedList.Add($"{name} - {Math.Round(mealTotalKcal)} kcal");
            }

            return formattedList;
        }
    }
}
