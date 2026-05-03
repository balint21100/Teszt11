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

        public ICollection<Food> GetFood()
        {
            var foods = foodDataProvider.GetFoods();
            return foods;
        }

        public void AddMealWithEntry(int userId, string mealName, int foodId, float quantity)
        {
            // 1. Létrehozzuk az alap étkezést
            var newMeal = new Meal
            {
                User_Id = userId,
                Name = mealName,
                Date = DateTime.Now // A mentés pillanatnyi ideje
            };

            // Elmentjük a MealDataProvider segítségével, és visszakapjuk a generált ID-t
            var savedMeal = mealDataProvider.AddMeal(newMeal);

            // 2. Létrehozzuk a tétel bejegyzést, ami összeköti a Meal-t és a Food-ot
            var newEntry = new MealEntry
            {
                Meal_Id = savedMeal.Id, // Az imént mentett étkezés azonosítója
                Food_Id = foodId,       // A kiválasztott kaja azonosítója
                Qty = quantity          // A megadott mennyiség
            };

            // Elmentjük a tételt a MealEntryDataProvider segítségével
            mealEntryDataProvider.AddMealEntry(newEntry);
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
        public List<GrafikonAdatDto> GetStatisztika(int userId, string tipus)
        {
            var meals = mealDataProvider.GetMeals(userId);
            var result = new List<GrafikonAdatDto>();

            if (tipus == "Heti")
            {
                // Utolsó 7 nap, naponkénti bontás
                var start = DateTime.Now.Date.AddDays(-6);
                for (var d = start; d <= DateTime.Now.Date; d = d.AddDays(1))
                {
                    result.Add(new GrafikonAdatDto
                    {
                        Ertek = SzamolNapiKaloria(userId, d),
                        Name = d.ToString("ddd") // H, K, Sze...
                    });
                }
            }
            else if (tipus == "Havi")
            {
                // Megszerezzük a jelenlegi dátumot
                var most = DateTime.Now;
                // Megtudjuk, hány napos a jelenlegi hónap (28, 29, 30 vagy 31)
                int napokAHonapban = DateTime.DaysInMonth(most.Year, most.Month);

                for (int nap = 1; nap <= napokAHonapban; nap++)
                {
                    DateTime aktualisNap = new DateTime(most.Year, most.Month, nap);

                    result.Add(new GrafikonAdatDto
                    {
                        Ertek = SzamolNapiKaloria(userId, aktualisNap),
                        // A Name lesz az, ami a grafikon alatt megjelenik
                        // Csak minden 2. vagy 5. napot érdemes feliratozni, hogy ne érjenek össze a számok
                        Name = nap % 2 == 0 ? nap.ToString() : ""
                    });
                }
            }
            else if (tipus == "Eves")
            {
                // Utolsó 12 hónap, havi bontásban
                int aktualisEv = DateTime.Now.Year;

                for (int honap = 1; honap <= 12; honap++)
                {
                    // Kiszámoljuk az adott hónap összes kalóriáját
                    float haviErtek = SzamolHaviKaloria(userId, aktualisEv, honap);

                    // Létrehozunk egy dátumot csak azért, hogy a nevét (Jan, Feb...) megkapjuk
                    DateTime honapDatum = new DateTime(aktualisEv, honap, 1);

                    result.Add(new GrafikonAdatDto
                    {
                        Ertek = haviErtek,
                        Name = honapDatum.ToString("MMM"), // Jan, Feb, Már... felirat
                        Datum = honapDatum
                    });
                }
            }
            return result;
        }

        private float SzamolNapiKaloria(int userId, DateTime datum)
        {
            float napiOsszesKcal = 0;

            // 1. Megkeressük az adott felhasználó adott napi étkezéseit
            var napiMeals = mealDataProvider.GetMeals(userId)
                .Where(m => m.Date.Date == datum.Date)
                .ToList();

            // 2. Végigmegyünk az étkezéseken (Reggeli, Ebéd, stb.)
            foreach (var meal in napiMeals)
            {
                // 3. Lekérjük az étkezéshez tartozó ételeket (tételeket)
                var entries = mealEntryDataProvider.GetMealEntries(meal.Id);

                foreach (var entry in entries)
                {
                    // 4. Megkeressük az étel adatait a kalóriaérték miatt
                    var food = foodDataProvider.GetFoods()
                        .FirstOrDefault(f => f.Id == entry.Food_Id);

                    if (food != null)
                    {
                        // Számolunk: (kalória / 100) * mennyiség
                        napiOsszesKcal += (food.Kcal / 100f) * entry.Qty;
                    }
                }
            }

            return napiOsszesKcal;
        }

        private float SzamolHaviKaloria(int userId, int year, int month)
        {
            float haviOsszesen = 0;

            // Megnézzük, hány nap van az adott hónapban (pl. február 28 vagy 29)
            int napokSzama = DateTime.DaysInMonth(year, month);

            // Végigmegyünk a hónap minden egyes napján
            for (int nap = 1; nap <= napokSzama; nap++)
            {
                DateTime aktualisNap = new DateTime(year, month, nap);

                // Meghívjuk a már megírt napi számolót és hozzáadjuk a havihoz
                haviOsszesen += SzamolNapiKaloria(userId, aktualisNap);
            }

            return haviOsszesen;
        }


        // MealService.cs - Add hozzá ezt
        public List<GrafikonAdatDto> GetKaloriaStatisztika(int userId, int napokSzama)
        {
            var hatarido = DateTime.Now.AddDays(-napokSzama);
            var meals = mealDataProvider.GetMeals(userId)
                .Where(m => m.Date >= hatarido)
                .ToList();

            var result = new List<GrafikonAdatDto>();
            var csoportositott = meals.GroupBy(m => m.Date.Date);

            foreach (var nap in csoportositott)
            {
                float napiKcal = 0;
                foreach (var meal in nap)
                {
                    var entries = mealEntryDataProvider.GetMealEntries(meal.Id);
                    foreach (var entry in entries)
                    {
                        var food = foodDataProvider.GetFoods().FirstOrDefault(f => f.Id == entry.Food_Id);
                        if (food != null) napiKcal += (food.Kcal * entry.Qty) / 100f;
                    }
                }
                result.Add(new GrafikonAdatDto { Datum = nap.Key, Ertek = napiKcal, Name = nap.Key.ToString("MM.dd") });
            }
            return result.OrderBy(x => x.Datum).ToList();
        }

        public Macros GetAtlagosMakrok(int userId, int napok)
        {
            var hatarido = DateTime.Now.AddDays(-napok);
            var meals = mealDataProvider.GetMeals(userId).Where(m => m.Date >= hatarido).ToList();

            float osszProt = 0, osszCh = 0, osszZsir = 0;
            if (!meals.Any()) return new Macros();

            foreach (var meal in meals)
            {
                var entries = mealEntryDataProvider.GetMealEntries(meal.Id);
                foreach (var entry in entries)
                {
                    var food = foodDataProvider.GetFoods().FirstOrDefault(f => f.Id == entry.Food_Id);
                    if (food != null)
                    {
                        osszProt += (food.Protein * entry.Qty) / 100f;
                        osszCh += (food.Carbs * entry.Qty) / 100f;
                        osszZsir += (food.Fat * entry.Qty) / 100f;
                    }
                }
            }

            int napokSzama = meals.Select(m => m.Date.Date).Distinct().Count();
            return new Macros
            {
                Protein = osszProt / napokSzama,
                Carbs = osszCh / napokSzama,
                Fat = osszZsir / napokSzama
            };
        }
    }
}
